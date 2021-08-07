using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Player : MonoBehaviour
{
    public Unit SelectedUnit { get; private set; }

    [field: SerializeField]
    public int Money { get; private set; } = 0;

    [SerializeField]
    [Tooltip("The units the player owns.")]
    List<Unit> playerUnits = new List<Unit>();

    [SerializeField]
    [Tooltip("The installations this payer owns.")]
    List<Installation> playerInstallations = new List<Installation>();

    [field: SerializeField]
    [field: Tooltip("The commander in use by this player")]
    public Commander PlayerCommander { get; private set; }

    private void Start()
    {
        //Mark each unit as being owned by this player.
        foreach(Unit unit in playerUnits)
        {
            if (unit == null)
                throw new System.NullReferenceException("Player has NULL unit reference");

            unit.TakeOwnership(this);
        }
    }


    public bool SelectUnit(Vector2Int position)
    {
        GameBoard gameBoard = GameController.gameController.gameBoard;
        Unit targetUnit = gameBoard.GetTile(position).unit;

        if (gameBoard.Contains(position))
        {
            if(Owns(targetUnit))
            {
                SelectedUnit = targetUnit;
            }
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
            unit.ReadyUnit();
        }

        GameController.gameController.EndTurn(this);
    }

    public bool Owns(Unit targetUnit)
    {
        foreach(Unit unit in playerUnits)
        {
            if (unit == targetUnit)
                return true;
        }


        return false;
    }

    public bool Owns(Installation targetInstallation)
    {
        foreach (Installation installation in playerInstallations)
        {
            if (installation == targetInstallation)
                return true;
        }
        return false;
    }


    public void UpdateCash()
    {
        foreach(Installation installation in playerInstallations)
        {
            Money += installation.Income;
        }
    }
    
    public void RemoveUnit(Unit unit)
    {
        playerUnits.Remove(unit);
    }

    public void RemoveInstallation(Installation installation)
    {
        playerInstallations.Remove(installation);
    }

    public void AddInstallation(Installation installation)
    {
        playerInstallations.Add(installation);
    }

    public void PurchaseUnit(Unit unitPrefab, Vector3 position)
    {
        //If we don't have enough money we're done.
        if (Money < unitPrefab.Cost)
            return;

        Money -= unitPrefab.Cost;

        GameObject newUnitGameObject = Instantiate(unitPrefab.gameObject, position, unitPrefab.gameObject.transform.rotation);
        Unit  newUnit = newUnitGameObject.GetComponent<Unit>();
        newUnit.TakeOwnership(this);
        playerUnits.Add(newUnit);
    }
}
