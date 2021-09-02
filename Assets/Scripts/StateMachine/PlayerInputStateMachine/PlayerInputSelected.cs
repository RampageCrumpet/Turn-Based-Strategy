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

        List<GameObject> tileDisplayIndicators = new List<GameObject>();
        RangeFinder rangeFinder = GameController.gameController.rangeFinder;
        AStar pathfinder;
        GameBoard gameBoard;
        Unit selectedUnit;

        public override void PrepareState()
        {
            base.PrepareState();

            player = owner.GetComponent<Player>();
            playerInputController = owner.GetComponent<PlayerInputController>();
            selectedUnit = player.SelectedUnit;
            pathfinder = GameController.gameController.pathfinder;
            gameBoard = GameController.gameController.gameBoard;


            CreateMovementIndicators();
            CreateAttackIndicators();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            //If it's not our turn we don't want to do anything.
            if (!GameController.gameController.IsTurn(playerInputController.LocalPlayer))
            {
                owner.ChangeState(new PlayerInputUnselected());
                return;
            }


            Vector2Int tilePosition = GameController.gameController.gameBoard.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //Deselect our unit if we rightclick.
            if (Input.GetMouseButtonDown(1))
                owner.ChangeState(new PlayerInputUnselected());

            //If we click an empty space we want to move there, if we click a unit we want to display the UnitMenu.
            if (Input.GetMouseButtonDown(0))
            {
                Path path = pathfinder.FindPath(player.SelectedUnit.Position, tilePosition, MovementType.TrueDistance);

                //We clicked an empty space, we want to move to it. 
                bool clickedOnEmptyTile = gameBoard.GetTile(tilePosition).unit == null;

                if (clickedOnEmptyTile)
                {
                    //If the player successfully moved.
                    if(player.SelectedUnit.CanFollow(path))
                    {
                        playerInputController.IssueMoveOrder(tilePosition);
                        owner.ChangeState(new PlayerInputMoved());
                    }
                    else //Something blocked our move. Deselect our tile.
                    {
                        owner.ChangeState(new PlayerInputUnselected());
                    }
                        
                }
                else //We clicked on a unit.
                {
                    //We clicked on ourselves and don't want to attack. Instead we want to open the UnitMenu.
                    if(selectedUnit == gameBoard.GetTile(tilePosition).unit)
                    {
                        owner.ChangeState(new PlayerInputMoved());
                        return;
                    }

                    //If we have no weapon deselect our unit.
                    if(selectedUnit.unitWeapons == null)
                    {
                        owner.ChangeState(new PlayerInputUnselected());
                        return;
                    }

                    foreach(Weapon weapon in selectedUnit.unitWeapons)
                    {
                        //If the target is already in range of any of our weapons attack it. 
                        if (path.Cost <= weapon.maximumRange && path.Cost >= weapon.minimumRange)
                        {
                            //Attack the unit, deselect this unit and then leave the function
                            playerInputController.IssueAttackOrder(tilePosition);
                        }
                        else //Otherwise we want to the cheapest square adjacent to our target.
                        {
                            Vector2Int cheapestNeighbor = FindCheapestNeighbor(tilePosition);

                            playerInputController.IssueMoveOrder(cheapestNeighbor);
                            playerInputController.IssueAttackOrder(tilePosition);
                        }
                    }

                    owner.ChangeState(new PlayerInputUnselected());
                }
            }

        }

        public override void DestroyState()
        {
            foreach(GameObject tileDisplayIndicator in tileDisplayIndicators)
            {
                tileDisplayIndicator.SetActive(false);
            }
            base.DestroyState();
        }

        private void CreateMovementIndicators()
        {
            //Don't create any movement indicators if we can't move
            if (!selectedUnit.CanMove)
                return;

            List<Vector2Int> movementLocations = rangeFinder.GetTilesWithinRange(selectedUnit.Position,
            0, selectedUnit.movement, selectedUnit.movementType).ToList();

            //Create all of the tile indicators.
            foreach (Vector2Int boardPosition in movementLocations)
            {
                //Set up the sprite and place it in the world.
                if (gameBoard.GetTile(boardPosition).unit != null)
                    continue;
                
                GameObject tileIndicator = ObjectPooler.objectPooler.GetPooledObject("TileIndicator");


                tileDisplayIndicators.Add(tileIndicator);

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

                attackLocations.UnionWith(rangeFinder.GetTilesWithinRange(selectedUnit.Position, min, max, selectedUnit.movementType));
            }

            //Create the attack indicators
            foreach (Vector2Int boardPosition in attackLocations)
            {
                //Set up the sprite and place it in the world.
                if (gameBoard.GetTile(boardPosition).unit != null)
                {
                    GameObject tileIndicator = ObjectPooler.objectPooler.GetPooledObject("TileIndicator");

                    tileDisplayIndicators.Add(tileIndicator);
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
            Path cheapestNeighborPath = pathfinder.FindPath(player.SelectedUnit.Position, cheapestNeighborPosition, player.SelectedUnit.movementType);

            //For each cardinal direction
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //Only Check the 4 cardinal directions. 
                    if (Mathf.Abs(x) + Mathf.Abs(y) != 1)
                        continue;

                    Vector2Int offset = new Vector2Int(x, y);
                    Path neighborPath = pathfinder.FindPath(player.SelectedUnit.Position, tilePosition + offset, player.SelectedUnit.movementType);

                    //if the neighbor we explored takes more to get to than our current cheapest check the next one.
                    if (neighborPath.Cost > cheapestNeighborPath.Cost)
                        continue;

                    cheapestNeighborPath = neighborPath;
                    cheapestNeighborPosition = tilePosition + offset;
                }
            }

            return cheapestNeighborPosition;
        }
    }
}
