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

        //If we're the server we want to add them to our GameControllers list of players.
        if(isServer)
        {
            GameController.gameController.AddPlayer(this);
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
        //If it's not this players turn don't issue the order.
        if (!GameController.gameController.IsTurn(this))
            return;

        unitObject.GetComponent<Unit>().ServerMove(position);
    }

    [Command]
    public void CmdIssueAttackOrder(Vector2Int position, GameObject unitObject)
    {
        //If it's not this players turn don't issue the order.
        if (!GameController.gameController.IsTurn(this))
            return;

        unitObject.GetComponent<Unit>().ServerAttack(position);
    }

    [Command]
    public void CmdEndTurn()
    {
        //If it's not this players turn don't try to end the turn.
        if (!GameController.gameController.IsTurn(this))
            return;

        GameController.gameController.ServerEndTurn(this);

        //Allow each unit to move again next turn.
        foreach(Unit unit in playerUnits)
        {
            unit.ReadyUnit();
        }
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


 
    public void UpdateCash()
    {
        foreach(Installation installation in playerInstallations)
        {
            Money += installation.Income;
        }

    }
    
 
    public void RemoveUnit(GameObject unitObject)
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
    public void CmdPurchaseUnit(String unitName, Vector3 position)
    {
        //If it's not this players turn don't execute the ability. 
        if (!GameController.gameController.IsTurn(this))
            return;

        Unit unitPrefab = null;

        foreach (Unit purchasableUnit in PlayerCommander.ConstructionPrefabs)
        {
            if(purchasableUnit.name == unitName)
            {
                unitPrefab = purchasableUnit;
            }
        }

        if(unitPrefab == null)
        {
            Debug.LogError("No constructable unit with name " + unitName + " can be found.");
            return;
        }
        
        //If we don't have enough money we're done.
        if (Money < unitPrefab.Cost)
            return;

        Money -= unitPrefab.Cost;


        //Create the new unit and spawn it across the network.
        Unit newUnit = Instantiate(unitPrefab.gameObject, position, unitPrefab.gameObject.transform.rotation).GetComponent<Unit>();
        NetworkServer.Spawn(newUnit.gameObject);

        if(isServerOnly)
        {
            playerUnits.Add(newUnit);
            newUnit.TakeOwnership(this);
        }

        RpcAddNewUnit(newUnit.gameObject);
    }

    [ClientRpc]
    public void RpcAddNewUnit(GameObject newUnitObject)
    {
        Unit unit = newUnitObject.GetComponent<Unit>();
        unit.TakeOwnership(this);
        playerUnits.Add(unit);
    }

    [Command]
    public void CmdExecuteAbility(String abilityName, GameObject invokingUnitGameObject)
    {
        //If it's not this players turn don't execute the ability. 
        if (!GameController.gameController.IsTurn(this))
            return;

        Ability ability = null;

        if (invokingUnitGameObject == null)
        {
            Debug.LogError("Error: Executing ability on NULL unit..");
            return;
        }
        Unit invokingUnit = invokingUnitGameObject.GetComponent<Unit>();

        if (invokingUnit == null)
        {
            Debug.LogError("Error: Cannot find unit component on " + invokingUnitGameObject.name);
            return;
        }

        foreach (Ability unitAbility in invokingUnit.abilities)
        {
            if(unitAbility.name == abilityName)
            {
                ability = unitAbility;
            }
        }
        if (ability == null)
        {
            Debug.LogError("Error: Executing NULL ability.");
            return;
        }


        if (!invokingUnit.abilities.Contains(ability))
        {
            Debug.LogError("Error: Executing unit" + SelectedUnit.name + " does not have " + ability.name);
            return;
        }

        //If we're running a dedicated server we need to execute the ability locally.
        if (isServerOnly && ability.CanExecute())
            ability.Execute();


        //Execute the ability on all of the connected clients.
        RpcExecuteAbility(abilityName, invokingUnitGameObject);
    }

    [ClientRpc]
    public void RpcExecuteAbility(String abilityName, GameObject invokingUnitGameObject)
    {
        Ability ability = null;

        Unit invokingUnit = invokingUnitGameObject.GetComponent<Unit>();


        foreach (Ability unitAbility in invokingUnit.abilities)
        {
            if (unitAbility.name == abilityName)
            {
                ability = unitAbility;
            }
        }



        if (ability == null)
        {
            Debug.LogError(SelectedUnit.name + " is attempting to execute " + abilityName + " but unit doesn't have an ability with that name.");
            return;
        }

        if(invokingUnit == null)
        {
            Debug.LogError("No selected unit to activate ability " + abilityName);
        }

        ability.Execute();

        Debug.Log("Invoking: " + abilityName);
    }
}
