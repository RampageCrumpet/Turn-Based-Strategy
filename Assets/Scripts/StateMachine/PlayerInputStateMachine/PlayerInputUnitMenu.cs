using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputUnitMenu : BaseState
{
    Player player;
    PlayerInputController playerInputController;
    UnitMenu unitMenu;

    public override void PrepareState()
    {
        base.PrepareState();

        player = owner.GetComponent<Player>();
        playerInputController = owner.GetComponent<PlayerInputController>();
        unitMenu = playerInputController.GetComponent<UnitMenu>();

        //Hook the UnitMenu buttons up to functions
        unitMenu.InitializeButtons(UnitMenuAttack, UnitMenuWait,
             UnitMenuCancel, UnitMenuSpecial);

        playerInputController.unitMenu.ShowUnitMenu(Input.mousePosition, false);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (Input.GetMouseButtonDown(1))
        {
            UnitMenuWait();
        }
    }

    public override void DestroyState()
    {
        player.DeselectUnit();
        unitMenu.HideUnitMenu();
        base.DestroyState();
    }

    private void UnitMenuCancel()
    {

    }

    private void UnitMenuAttack()
    {

    }

    private void UnitMenuSpecial()
    {

    }

    private void UnitMenuWait()
    {
        owner.ChangeState(new PlayerInputUnselected());
        player.DeselectUnit();
        unitMenu.HideUnitMenu();
    }
}
