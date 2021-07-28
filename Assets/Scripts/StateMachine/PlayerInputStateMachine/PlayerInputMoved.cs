using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMoved : BaseState
{
    Player player;
    PlayerInputController playerInputController;

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
    }
}
