using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    Unit selectedUnit = null;

    [SerializeField]
    [Tooltip("The players starting cash.")]
    private int money = 0;

    [SerializeField]
    [Tooltip("The units the player owns.")]
    List<Unit> units = new List<Unit>();


    private void IssueCommand()
    {

    }



    public bool SelectUnit(Vector2Int position)
    {
        GameBoard gameBoard = GameController.gameController.gameBoard;

        if (gameBoard.Contains(position))
        {
            selectedUnit = gameBoard.GetTile(position).unit;
        }
        else
        {
            Debug.LogWarning("Attempting to select a unit outside of the map bounds.");
        }



        //DEBUG STUFF:
        if (selectedUnit == null)
            Debug.Log("Selected NULL");
        else
            Debug.Log("Selected Unit:" + selectedUnit.name);

        return false;
    }


    public Unit GetSelectedUnit() {  return selectedUnit; }

    public void DeselectUnit()
    {
        selectedUnit = null;
    }

    public void IssueMoveOrder(Vector2Int position)
    {
        selectedUnit.Move(position);
    }

    public void EndTurn()
    {
        GameController.gameController.EndTurn(this);
    }
}
