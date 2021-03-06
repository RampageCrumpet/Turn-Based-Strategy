using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PlayerInputStateMachine
{ 
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
        unitMenu = playerInputController.UnitMenu;
        unitMenu.playerInputController = playerInputController;

        //Hook the UnitMenu buttons up to functions
        unitMenu.InitializeStandardButtons(UnitMenuAttack, UnitMenuWait, UnitMenuCancel);

        //Initialize all of the ability specific buttons.
        foreach (Ability ability in player.SelectedUnit.abilities)
        {
            if(ability.CanExecute())
                unitMenu.InitializeAbilityButton(ability, UnitMenuSpecial);
        }

        playerInputController. UnitMenu.ShowUnitMenu(Input.mousePosition, false);
    }

    public override void UpdateState()
    {
        //If it's not our turn we don't want to do anything.
        if (!GameController.gameController.IsTurn(player))
        {
            owner.ChangeState(new PlayerInputUnselected());
            return;
        }
            

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
            Debug.LogError("UnitMenuAttack is still a method stub.");
    }

    private void UnitMenuSpecial()
    {
            owner.ChangeState(new PlayerInputUnselected());
            player.DeselectUnit();
    }

    private void UnitMenuWait()
    {
        owner.ChangeState(new PlayerInputUnselected());
        player.DeselectUnit();
    }
}
}