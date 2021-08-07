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
            constructionMenu = playerInputController.constructionMenu;
            position = (Camera.main.ScreenToWorldPoint(Input.mousePosition)).GetGridsnapPosition();

            //Initialize all of the buttons in the construction menu.
            foreach (Unit unit in player.PlayerCommander.ConstructionPrefabs)
            {
                constructionMenu.InitializeConstructionButton(player.PurchaseUnit, ChangeState, unit, position);
            }

            constructionMenu.ShowConstructionMenu();
        }

        public override void UpdateState()
        {
            base.UpdateState();

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
