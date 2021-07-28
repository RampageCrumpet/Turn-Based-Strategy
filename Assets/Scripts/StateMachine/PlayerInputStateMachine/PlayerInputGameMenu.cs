using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputGameMenu : BaseState
{
    PlayerInputController inputController;

    public override void PrepareState()
    {
        base.PrepareState();

        inputController = owner.GetComponent<PlayerInputController>();

        //Register our destroy state with the buttons.
        inputController.gameMenu.InitializeButtons(DestroyState, DestroyState);
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            owner.ChangeState(new PlayerInputUnselected());
        }
    }

    public override void DestroyState()
    {
        inputController.gameMenu.HideGameMenu();
        base.DestroyState();
    }
}
