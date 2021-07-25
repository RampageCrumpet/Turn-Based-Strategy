using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(PathFollower))]
public class Unit : MonoBehaviour
{
    [Flags]
    public enum UnitType { Infantry = 1, Vehicle = 2, Helicopter = 4, Boat = 8, Aircraft = 16}


    //Control Scripts
    PathFollower pathFollower;

    //Events
    public delegate void MoveUnit(Unit unit, Vector2Int startingPosition, Vector2Int targetPosition);
    public static event MoveUnit OnMove;


    [SerializeField]
    [Tooltip("The Units name displayed in game.")]
    string unitName = "Unnamed Unit";

    [Header("Unit Statistics")]
    [SerializeField]
    [Tooltip("How many tiles a unit can move in one turn.")]
    uint movement = 1;

    [SerializeField]
    [Tooltip("How many tiles a unit can see in fog of war.")]
    uint vision = 1;

    [SerializeField]
    [Tooltip("How many tiles away a unit can shoot.")]
    uint maxRange = 1;

    [SerializeField]
    [Tooltip("How many tiles away an enemy has to be so this unit can shoot it.")]
    uint minRange = 1;

    [SerializeField]
    [Tooltip("The movement type used by the unit.")]
    MovementType movementType;

    [SerializeField]
    [Tooltip("This units type. Used for determining which weapons can attack it.")]
    UnitType unitType;

    [SerializeField]
    [Tooltip("The weapons this unit can use to attack.")]
    List<Weapon> unitWeapons;


    //The remaining hitpoints on the unit.
    int hitPoints= 10;

    //A flag used to see if a unit has exhausted itself yet.
    public bool CanMove { get;  set;} = false;




    // Start is called before the first frame update
    void Start()
    {
        pathFollower = this.GetComponent<PathFollower>();

        //Set the starting position of the unit on the gameBoard.
        Vector2Int position = GameController.gameController.gameBoard.WorldToCell(this.transform.position);

        GameController.gameController.AddUnitToGameBoard(this, position);
    }

    public void Move(Vector2Int targetPosition)
    {
        

        Vector2Int startPosition = GameController.gameController.gameBoard.WorldToCell(this.transform.position);
        Stack <Vector2Int> path = GameController.gameController.pathfinder.FindPath(startPosition, targetPosition, movementType);

        //Trigger observers to watch for this unit moving. OnMove should never be null.
        OnMove(this, startPosition, targetPosition);

        pathFollower.FollowPath(path);
    }

    public void Attack(Unit target)
    {
        //We can't attack with an undefined set of weapons.
        if (unitWeapons == null)
            return;
    }


}
