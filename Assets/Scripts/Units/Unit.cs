using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using ExtensionMethods;
using Mirror;

[RequireComponent(typeof(PathFollower))]
public class Unit : NetworkBehaviour
{
    [Flags]
    public enum UnitType { Infantry = 1, Vehicle = 2, Helicopter = 4, Boat = 8, Aircraft = 16}
    [Flags]
    public enum AbilityFlag { Capture = 1}


    //Control Scripts
    PathFollower pathFollower;

    //Events
    public delegate void MoveUnit(GameObject unitObject, Vector2Int startingPosition, Vector2Int targetPosition);
    public static event MoveUnit OnMove;


    [field: SerializeField]
    [field: Tooltip("The Units name displayed in game.")]
    public string UnitName { get; private set; } = "Unnamed Unit";

    [Space(10)]
    [Header("Unit Statistics")]
    [field :SerializeField]
    [Tooltip("How many tiles a unit can move in one turn.")]
    public uint movement = 1;

    [SerializeField]
    [Tooltip("How many tiles a unit can see in fog of war.")]
    uint vision = 1;


    [SerializeField]
    [Tooltip("The units ability to resist damage.")]
    public int Armor;

    [SerializeField]
    [Tooltip("The movement type used by the unit.")]
    public MovementType movementType;

    [SerializeField]
    [Tooltip("This units type. Used for determining which weapons can attack it.")]
    UnitType unitType;

    [field: SerializeField]
    [field: Tooltip("The cost to deploy this unit at a factory.")]
    public int Cost { get; private set; } = 100;

    [Space(10)]
    [Header("Armarments")]
    [SerializeField]
    [Tooltip("The weapons this unit can use to attack.")]
    public List<Weapon> unitWeapons;

    //Actually a list of flags used to create the abilities. Unity doesn't support polymorphism in the inspector.
    [SerializeField]
    [Tooltip("The special abilities this unit has access to.")]
    AbilityFlag abilityFlags;
    

    public List<Ability> abilities = new List<Ability>();


    //The remaining hitpoints on the unit.
    [field: SyncVar]
    public int HitPoints { get; private set;} = 10;


    //A flag used to see if a unit has exhausted itself yet.
    public bool CanMove { get; private set;} = true;


    public Vector2Int Position { get; protected set; }

    public Player Player { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        InitializeUnit();
    }

    private void InitializeUnit()
    {
        pathFollower = this.GetComponent<PathFollower>();

        this.transform.position = this.transform.position.GetGridsnapPosition();

        //Set the starting position of the unit on the gameBoard.
        Position = GameController.gameController.gameBoard.WorldToCell(this.transform.position);


        GameController.gameController.AddUnitToGameBoard(this, Position);

        CreateAbilities();
    }

 


    [Server]
    public void ServerMove(Vector2Int targetPosition)
    {
        //We need to update the position of the unit on server only setups.
        if(isServerOnly)
            UpdatePosition(targetPosition);

        RpcMove(targetPosition);
        
    }

    [ClientRpc]
    public void RpcMove(Vector2Int targetPosition)
    {
        //If we're already at the target position we don't need to move.
        if (this.Position == targetPosition)
            return;


        //Create the path and order the Unit's visualization to follow it.
        Vector2Int startPosition = GameController.gameController.gameBoard.WorldToCell(this.transform.position);
        Path path = GameController.gameController.pathfinder.FindPath(startPosition, targetPosition, movementType, Player);
        pathFollower.FollowPath(path);

        UpdatePosition(targetPosition);
    }

    [Server]
    public void ServerAttack(Vector2Int position)
    {
        Unit target = GameController.gameController.gameBoard.GetTile(position).unit;
        int targetDefense = GameController.gameController.gameBoard.GetTile(position).defense;

        if(isServerOnly)
        {
            AttackTarget(target, targetDefense);
        }
            

        RpcAttackTarget(target.gameObject, targetDefense);
    }

    [ClientRpc]
    public void RpcAttackTarget(GameObject targetGameObject, int targetDefense)
    {
        Unit targetUnit = targetGameObject.GetComponent<Unit>();
        AttackTarget(targetUnit, targetDefense);
    }

    private void AttackTarget(Unit targetUnit, int targetDefense)
    {
        //We can't attack with an undefined set of weapons.
        if (unitWeapons == null)
            return;

        foreach (Weapon weapon in unitWeapons)
        {
            targetUnit.TakeDamage(weapon.CalculateDamage(HitPoints, targetUnit.Armor, targetDefense));
        }
    }

    public void TakeDamage(int damageTaken)
    {
        HitPoints -= damageTaken;
        Debug.Log("Unit has taken " + damageTaken);
    }

    //Readies a unit to take another turn.
    public void ReadyUnit()
    {
        CanMove = true;
    }

    private void CreateAbilities()
    {
        if((abilityFlags & AbilityFlag.Capture) != 0)
        {
            Capture capture = new Capture();
            capture.Initialize(this, GameController.gameController.gameBoard);
            abilities.Add(capture);
        }
    }

    public void TakeOwnership(Player player)
    {
        this.Player = player;
    }

    public bool CanFollow(Path path)
    {
        if (CanMove == false)
            return false;

        //If we failed to generate a path we want to return false. We will not be following the path.
        if (!path.isPathable)
            return false;

        //If the path is longer than we're allowed to remove we want to return false. We wont be following the path.
        if (path.Cost > movement)
            return false;

        return true;
    }

    private void UpdatePosition(Vector2Int targetPosition)
    {
        //Trigger observers to watch for this unit moving. OnMove should never be null.
        OnMove(this.gameObject, Position, targetPosition);

        //Update our recorded position.
        Position = targetPosition;
    }

}
