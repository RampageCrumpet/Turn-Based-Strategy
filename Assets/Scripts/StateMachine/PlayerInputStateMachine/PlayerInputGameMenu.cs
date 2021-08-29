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

            inputController = owner.GetComponent<PlayerInputController>();

            //Register our destroy state with the buttons.
            inputController.GameMenu.InitializeButtons(CloseMenu, CloseMenu);
        }

        public override void UpdateState()
        {

        }

        public override void DestroyState()
        {
            inputController.GameMenu.HideGameMenu();
            base.DestroyState();
        }

        public void CloseMenu()
        {
            owner.ChangeState(new PlayerInputUnselected());
        }
    }
}
