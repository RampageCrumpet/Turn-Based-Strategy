using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;



namespace PlayerInputStateMachine
{
    public class PlayerInputConstructionMenu : BaseState
    {
        PlayerInputController playerInputController;
        ConstructionMenu constructionMenu;
        Vector3 position;

        public override void PrepareState()
        {
            base.PrepareState();

            playerInputController = owner.GetComponent<PlayerInputController>();
            Player player = playerInputController.gameObject.GetComponent<Player>();
            constructionMenu = playerInputController.ConstructionMenu;
            position = (Camera.main.ScreenToWorldPoint(Input.mousePosition)).GetGridsnapPosition();
            position.z = 0; //Set the Z position to zero so it doesn't appear behind the tilemap.

            //Initialize all of the buttons in the construction menu.
            foreach (Unit unit in player.PlayerCommander.ConstructionPrefabs)
            {
                constructionMenu.InitializeConstructionButton(player.CmdPurchaseUnit, ChangeState, unit, position);
            }

            constructionMenu.ShowConstructionMenu();
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


            if (Input.GetMouseButtonDown(1))
                ChangeState();
        }

        public override void DestroyState()
        {
            constructionMenu.HideConstructionMenu();
            base.DestroyState();
        }

        public void ChangeState()
        {
            owner.ChangeState(new PlayerInputUnselected());
        }
    }
}
