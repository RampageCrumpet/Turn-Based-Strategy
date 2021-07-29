using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInputStateMachine
{
    public class PlayerInputGameMenu : BaseState
    {
        PlayerInputController inputController;

        public override void PrepareState()
        {
            base.PrepareState();

            Debug.Log("GameMenu");

            inputController = owner.GetComponent<PlayerInputController>();

            //Register our destroy state with the buttons.
            inputController.gameMenu.InitializeButtons(CloseMenu, CloseMenu);
        }

        public override void UpdateState()
        {
            if (Input.GetMouseButtonDown(1))
            {
                
            }
        }

        public override void DestroyState()
        {
            inputController.gameMenu.HideGameMenu();
            base.DestroyState();
        }

        public void CloseMenu()
        {
            owner.ChangeState(new PlayerInputUnselected());
        }
    }
}
