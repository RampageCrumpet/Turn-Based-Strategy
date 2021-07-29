using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

namespace PlayerInputStateMachine
{
    public class PlayerInputSelected : BaseState
    {
        Player player;
        PlayerInputController playerInputController;

        List<GameObject> tileDisplaIndicators = new List<GameObject>();
        RangeFinder rangeFinder = GameController.gameController.rangeFinder;
        AStar pathfinder;
        GameBoard gameBoard;
        Unit unit;

        public override void PrepareState()
        {
            base.PrepareState();

            player = owner.GetComponent<Player>();
            playerInputController = owner.GetComponent<PlayerInputController>();
            unit = player.SelectedUnit;
            pathfinder = GameController.gameController.pathfinder;
            gameBoard = GameController.gameController.gameBoard;


            CreateMovementIndicators();
            CreateAttackIndicators();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector2Int tilePosition = GameController.gameController.gameBoard.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //Deselect our unit if we rightclick.
            if (Input.GetMouseButtonDown(1))
                owner.ChangeState(new PlayerInputUnselected());

            //If we click an empty space we want to move there, if we click a unit we want to display the UnitMenu.
            if (Input.GetMouseButtonDown(0))
            {
                //We clicked an empty space, we want to move to it. 
                bool clickedOnEmptyTile = gameBoard.GetTile(tilePosition).unit == null;

                if (clickedOnEmptyTile)
                {
                    //If the player successfully moved.
                    if(player.IssueMoveOrder(tilePosition))
                    {
                        owner.ChangeState(new PlayerInputMoved());
                    }
                    else //Something blocked our move. Deselect our tile.
                    {
                        owner.ChangeState(new PlayerInputUnselected());
                    }
                        
                }
                else //We clicked on a unit.
                {
                    //If we have no weapon deselect our unit.
                    if(unit.unitWeapons == null)
                    {
                        owner.ChangeState(new PlayerInputUnselected());
                        return;
                    }

                    
                    
                    Stack<Vector2Int> path = pathfinder.FindPath(player.SelectedUnit.Position, tilePosition, MovementType.TrueDistance);
                    int pathCost = pathfinder.GetPathCost(path, MovementType.TrueDistance);

                    foreach(Weapon weapon in unit.unitWeapons)
                    {
                        //If the target is already in range of any of our weapons attack it. 
                        if (pathCost <= weapon.maximumRange && pathCost >= weapon.minimumRange)
                        {
                            //Attack the unit, deselect this unit and then leave the function
                            player.IssueAttackOrder(tilePosition);
                            owner.ChangeState(new PlayerInputUnselected());
                            return;
                        }
                        else //Otherwise we want to the cheapest square adjacent to our target.
                        {
                            Vector2Int cheapestNeighbor = FindCheapestNeighbor(tilePosition);
                            player.IssueMoveOrder(cheapestNeighbor);
                            player.IssueAttackOrder(tilePosition);
                        }
                    }

                    owner.ChangeState(new PlayerInputUnselected());
                }
            }

        }

        public override void DestroyState()
        {
            base.DestroyState();

            foreach(GameObject tileDisplayIndicator in tileDisplaIndicators)
            {
                tileDisplayIndicator.SetActive(false);
            }
        }

        private void CreateMovementIndicators()
        {
            List<Vector2Int> movementLocations = rangeFinder.GetTilesWithinRange(unit.Position,
            0, unit.movement, unit.movementType).ToList();

            //Create all of the tile indicators.
            foreach (Vector2Int boardPosition in movementLocations)
            {
                //Set up the sprite and place it in the world.
                if (gameBoard.GetTile(boardPosition).unit != null)
                    continue;
                
                GameObject tileIndicator = ObjectPooler.objectPooler.GetPooledObject("TileIndicator");


                tileDisplaIndicators.Add(tileIndicator);

                tileIndicator.GetComponent<SpriteRenderer>().sprite = playerInputController.moveIndicatorSprite;

                //Set the tileIndicator to the right X/Y position
                tileIndicator.transform.position = gameBoard.CellToWorld(boardPosition);
                tileIndicator.SetActive(true);
            }
        }

        private void CreateAttackIndicators()
        {
            //If the unit has no weapons it doesn't need to place any attack indicators.
            if (player.SelectedUnit.unitWeapons == null)
                return;

            HashSet<Vector2Int> attackLocations = new HashSet<Vector2Int>();

            foreach(Weapon weapon in player.SelectedUnit.unitWeapons)
            {
                uint max = (uint)(weapon.maximumRange + (weapon.canFireAfterMoving ? player.SelectedUnit.movement : 0));
                uint min = (uint)(weapon.canFireAfterMoving ? 0 : weapon.minimumRange);

                attackLocations.UnionWith(rangeFinder.GetTilesWithinRange(unit.Position, min, max, unit.movementType));
            }

            //Create the attack indicators
            foreach (Vector2Int boardPosition in attackLocations)
            {
                //Set up the sprite and place it in the world.
                if (gameBoard.GetTile(boardPosition).unit != null)
                {
                    GameObject tileIndicator = ObjectPooler.objectPooler.GetPooledObject("TileIndicator");

                    tileDisplaIndicators.Add(tileIndicator);
                    tileIndicator.GetComponent<SpriteRenderer>().sprite = playerInputController.attackIndicatorSprite;

                    //Set the tileIndicator to the right X/Y position
                    tileIndicator.transform.position = GameController.gameController.gameBoard.CellToWorld(boardPosition);
                    tileIndicator.SetActive(true);
                }

            }
        }

        /// <summary>
        /// This method returns the tile that's easiest for the players selected unit to get to. 
        /// </summary>
        /// <param name="tilePosition"></param>
        /// <returns></returns>
        private Vector2Int FindCheapestNeighbor(Vector2Int tilePosition)
        {
            //Prime the search.

            Vector2Int cheapestNeighborPosition = tilePosition + new Vector2Int(1, 0);
            Stack<Vector2Int> primingPath = pathfinder.FindPath(player.SelectedUnit.Position, cheapestNeighborPosition, player.SelectedUnit.movementType);
            int cheapestNeighborCost = pathfinder.GetPathCost(primingPath, player.SelectedUnit.movementType);

            //For each cardinal direction
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //Only Check the 4 cardinal directions. 
                    if (Mathf.Abs(x) + Mathf.Abs(y) != 1)
                        continue;

                    Vector2Int offset = new Vector2Int(x, y);
                    Stack<Vector2Int> neighborPath = pathfinder.FindPath(player.SelectedUnit.Position, tilePosition + offset, player.SelectedUnit.movementType);
                    int neighborCost = pathfinder.GetPathCost(neighborPath, player.SelectedUnit.movementType);

                    //if the neighbor we explored takes more to get to than our current cheapest check the next one.
                    if (neighborCost > cheapestNeighborCost)
                        continue;

                    cheapestNeighborCost = neighborCost;
                    cheapestNeighborPosition = tilePosition + offset;
                }
            }

            return cheapestNeighborPosition;
        }
    }

}
