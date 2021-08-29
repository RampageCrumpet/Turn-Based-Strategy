using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;
using Mirror;

[RequireComponent(typeof(RangeFinder))]
[RequireComponent(typeof(AStar))]
[RequireComponent(typeof(GameBoard))]
public class GameController : NetworkBehaviour
{
    [field: SerializeField]
    public AStar pathfinder { get; protected set; }
    [field: SerializeField]

    public static GameController gameController { get; protected set; }


    public GameBoard gameBoard { get; protected set; }

    public RangeFinder rangeFinder { get; protected set; }


    [field: SerializeField]
    List<PlayerStartState> playerStartStates = new List<PlayerStartState>();

    List<Player> players = new List<Player>();

    public int DayCount { get; private set; } = 0;

    public Player ActivePlayer { get { return players[activePlayerIndex]; } private set { } }
    int activePlayerIndex = 0;

    void Awake()
    {
        //Enforce the singleton pattern
        if (gameController != null)
        {
            Debug.LogError("Multiple GameControllers detected in the scene.");
            Destroy(this);
        }
        else
        {
            gameController = this;
        }

        pathfinder = this.GetComponent<AStar>();
        rangeFinder = this.GetComponent<RangeFinder>();
        gameBoard = GameObject.FindObjectOfType<GameBoard>();
    }


    public void AddUnitToGameBoard(Unit unit, Vector2Int position)
    {
        if(gameBoard.Contains(position))
        {
            if(gameBoard.GetTile(position).unit == null)
            {
                gameBoard.GetTile(position).unit = unit;
            }
            else { 
            Debug.LogError("Attempting to add " + unit.name + " to " + position + " has failed. Tile not empty.");
            }
        }
        else
        {
            throw new System.ArgumentOutOfRangeException("GameBoard does not contain the position " + position.ToString());
        }
    }



    public void EndTurn(Player player)
    { 
        if(player == players[activePlayerIndex])
        {
            activePlayerIndex++;
            if (activePlayerIndex >= players.Count)
            {
                activePlayerIndex = 0;
                DayCount++;
            }
        }
    }

    public PlayerStartState GetStartState()
    {
        if(playerStartStates != null && playerStartStates.Count > 0)
        {
            PlayerStartState startState = playerStartStates[0];
            playerStartStates.Remove(startState);
            return startState;
        }
        else
        {
            Debug.LogError("Player Start States is null or empty. Please ensure start states have been created.");
        }

        return null;
    }

}
