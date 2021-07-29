using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace PlayerInputStateMachine
{
    public class PlayerInputMoved : BaseState
    {
        Player player;
        PlayerInputController playerInputController;
        List<GameObject> tileDisplaIndicators = new List<GameObject>();
        RangeFinder rangeFinder = GameController.gameController.rangeFinder;
        Unit unit;


        public override void PrepareState()
        {
            base.PrepareState();

            player = owner.GetComponent<Player>();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector2Int tilePosition = GameController.gameController.gameBoard.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //Deselect our unit if we rightclick.
            if (Input.GetMouseButtonDown(1))
            {
                player.DeselectUnit();
                owner.ChangeState(new PlayerInputUnselected());
            }

            //If we click open up the unit menu and change to it's state.
            if (Input.GetMouseButton(0))
            {
                owner.ChangeState(new PlayerInputUnitMenu());
                //TODO: This needs to show True if we can attack a unit. 
            }
        }

        public override void DestroyState()
        {
            base.DestroyState();

            foreach (GameObject tileDisplayIndicator in tileDisplaIndicators)
            {
                tileDisplayIndicator.SetActive(false);
            }
        }

        private void CreateAttackIndicators()
        {
            //If the unit has no weapons it doesn't need to place any attack indicators.
            if (player.SelectedUnit.unitWeapons == null)
                return;

            HashSet<Vector2Int> attackLocations = new HashSet<Vector2Int>();

            foreach (Weapon weapon in player.SelectedUnit.unitWeapons)
            {
                if (weapon.canFireAfterMoving == true)
                {
                    attackLocations.UnionWith(rangeFinder.GetTilesWithinRange(unit.Position,
                        (uint)weapon.minimumRange, (uint)weapon.maximumRange, unit.movementType));
                }
                else
                {
                    //We've already moved. If the unit can't fire after moving we don't need to make any target markers.
                    return;
                }
            }

            //Create the attack indicators
            foreach (Vector2Int boardPosition in attackLocations)
            {
                //Set up the sprite and place it in the world.
                if (GameController.gameController.gameBoard.GetTile(boardPosition).unit != null)
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
    }

}
