using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;


public class Player : NetworkBehaviour
{
    public Unit SelectedUnit { get; private set; }

    
    [field: Tooltip("The money the player currently has.")]
    [field: SerializeField]
    [field: SyncVar]
    public int Money { get; private set; } = 0;

    [SerializeField]
    [Tooltip("The units the player owns.")]
    List<Unit> playerUnits = new List<Unit>();

    [SerializeField]
    [Tooltip("The installations this payer owns.")]
    List<Installation> playerInstallations = new List<Installation>();

    [field: SyncVar]
    [field: SerializeField]
    [field: Tooltip("The commander in use by this player")]
    public Commander PlayerCommander { get; private set; }

    void Start()
    {
        PlayerStartState startState = GameController.gameController.GetStartState();

        Money = startState.Money;

        //Mark each unit as being owned by this player.
        foreach(Unit unit in startState.playerUnits)
        {
            //Set this units owner to this player.
            unit.TakeOwnership(this);
            playerUnits.Add(unit);
        }

        foreach (Installation installation in startState.playerInstallations)
        {
            //Set this units owner to this player.
            installation.CaptureInstallation(this);
            playerInstallations.Add(installation);
        }
    }




    public void SelectUnit(Vector2Int position)
    {
        GameBoard gameBoard = GameController.gameController.gameBoard;
        Unit targetUnit = gameBoard.GetTile(position).unit;

        if (gameBoard.Contains(position) && targetUnit != null)
        {
            if (Owns(targetUnit))
            {
                SelectedUnit = targetUnit;
            }
        }
    }



    public void DeselectUnit() => SelectedUnit = null;

    /// <summary>
    /// Sends a command to the server saying you want to move.
    /// </summary>
    /// <param name="position"></param>
    [Command]
    public void CmdIssueMoveOrder(Vector2Int position, GameObject unitObject)
    {
        unitObject.GetComponent<Unit>().ServerMove(position);
    }

    [Command]
    public void CmdIssueAttackOrder(Vector2Int position, GameObject unitObject)
    {
        unitObject.GetComponent<Unit>().ServerAttack(position);
    }

    [Command]
    public void CmdEndTurn()
    {
        //Allow each unit to move again next turn.
        foreach(Unit unit in playerUnits)
        {
            unit.ReadyUnit();
        }

        GameController.gameController.EndTurn(this);
    }


    //Checks if the player owns the target unit or installation
    public bool Owns(Installation targetInstallation)
    {
        foreach (Installation installation in playerInstallations)
        {
            if (installation == targetInstallation)
                return true;
        }
        return false;
    }

    public bool Owns(Unit targetUnit)
    {
        foreach (Unit unit in playerUnits)
        {
            if (unit == targetUnit)
                return true;
        }
        return false;
    }


    [Command]
    public void CmdUpdateCash()
    {
        foreach(Installation installation in playerInstallations)
        {
            Money += installation.Income;
        }

    }
    
    [Command]
    public void CmdRemoveUnit(GameObject unitObject)
    {
        Unit unit = unitObject.GetComponent<Unit>();
        playerUnits.Remove(unit);
    }

    public void RemoveInstallation(GameObject installationObject)
    {
        Installation installation = installationObject.GetComponent<Installation>();
        playerInstallations.Remove(installation);
    }

   
    public void AddInstallation(GameObject installationObject)
    {
        Installation installation = installationObject.GetComponent<Installation>();
        playerInstallations.Add(installation);
    }

    [Command]
    public void CmdPurchaseUnit(GameObject unitPrefabObject, Vector3 position)
    {
        Unit unitPrefab = unitPrefabObject.GetComponent<Unit>();

        //If we don't have enough money we're done.
        if (Money < unitPrefab.Cost)
            return;

        Money -= unitPrefab.Cost;

        GameObject newUnitGameObject = Instantiate(unitPrefab.gameObject, position, unitPrefab.gameObject.transform.rotation);
        NetworkServer.Spawn(newUnitGameObject);
        Unit  newUnit = newUnitGameObject.GetComponent<Unit>();
        newUnit.TakeOwnership(this);
        playerUnits.Add(newUnit);
    }
}
