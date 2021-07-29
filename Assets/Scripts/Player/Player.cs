using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public Unit SelectedUnit { get; private set; }

    [SerializeField]
    [Tooltip("The players starting cash.")]
    private int money = 0;

    [SerializeField]
    [Tooltip("The units the player owns.")]
    List<Unit> playerUnits = new List<Unit>();



    public bool SelectUnit(Vector2Int position)
    {
        GameBoard gameBoard = GameController.gameController.gameBoard;

        if (gameBoard.Contains(position))
        {
            SelectedUnit = gameBoard.GetTile(position).unit;
        }
        else
        {
            Debug.LogWarning("Attempting to select a unit outside of the map bounds.");
        }

        return false;
    }


    public void DeselectUnit() => SelectedUnit = null;

    public bool IssueMoveOrder(Vector2Int position)
    {
        return SelectedUnit.Move(position);
    }

    public void IssueAttackOrder(Vector2Int position)
    {
        SelectedUnit.AttackPosition(position);
    }

    public void EndTurn()
    {
        //Allow each unit to move again next turn.
        foreach(Unit unit in playerUnits)
        {
            unit.CanMove = true;
        }

        GameController.gameController.EndTurn(this);
    }
}
