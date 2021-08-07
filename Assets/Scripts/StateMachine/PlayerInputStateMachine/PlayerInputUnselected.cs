using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInputStateMachine
{
    public class PlayerInputUnselected : BaseState
    {
        Player player;
        PlayerInputController inputController;
        GameBoard gameBoard;


        public override void PrepareState()
        {
            base.PrepareState();

            player = owner.GetComponent<Player>();
            inputController = owner.GetComponent<PlayerInputController>();
            gameBoard = GameController.gameController.gameBoard;

            player.DeselectUnit();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector2Int tilePosition = GameController.gameController.gameBoard.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //Select the unit under the tile
            if (Input.GetMouseButtonDown(0))
            {
                //Select the unit in that tile
                player.SelectUnit(tilePosition);


                if (player.SelectedUnit != null)
                {
                    //We selected something.
                    owner.ChangeState(new PlayerInputSelected());
                }
                else
                {
                    UnitCreator unitCreator = gameBoard.GetTile(tilePosition).installation as UnitCreator;
                    if (unitCreator != null && player.Owns(unitCreator))
                    {
                        owner.ChangeState(new PlayerInputConstructionMenu());
                    }
                    else //If we didn't click on a unit producing installation
                    {
                        owner.ChangeState(new PlayerInputGameMenu());
                        inputController.gameMenu.ShowGameMenu(Input.mousePosition);
                    }


                        
                }
            }
        }

        public override void DestroyState()
        {
            base.DestroyState();
        }
    }
}