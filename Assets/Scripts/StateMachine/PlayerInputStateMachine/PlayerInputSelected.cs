using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSelected : BaseState
{
    Player player;

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

        //If we click an empty space we want to move there, if we click a unit we want to display the UnitMenu.
        if (Input.GetMouseButton(0))
        {
            //We clicked an empty space, we want to move to it. 
            if (GameController.gameController.gameBoard.GetTile(tilePosition).unit == null)
            {
                player.IssueMoveOrder(tilePosition);
                owner.ChangeState(new PlayerInputMoved());
            }
            else
            {
                //Display the UnitMenu
            }
        }

    }

    public override void DestroyState()
    {
        base.DestroyState();
    }
}
