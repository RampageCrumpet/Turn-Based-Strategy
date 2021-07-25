using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;


[RequireComponent(typeof(AStar))]
public class GameController : MonoBehaviour
{
    //Control scripts
    public AStar pathfinder;
    public static GameController gameController;
    public GameBoard gameBoard;



    List<Player> players = new List<Player>();


    
   


    void Awake()
    {
        //Enforce the singleton pattern
        if(gameController != null) {
            Debug.LogError("Multiple GameControllers detected in the scene.");
            Destroy(this); 
        }
        else {
            gameController = this; 
        }
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
            Debug.LogError("Adding unit " + unit.name + " to the gameboard at " + position.ToString() + " has failed. Outside Bounds.");
        }
    }

    public void EndTurn(Player player)
    {

    }


    /*public Unit GetUnitAt(Vector3Int position)
    {
        if (units.ContainsKey(position))
        {
            return units[position];
        }

        return null;
    }
    
    /// <summary>
    /// Perform the record keeping for unit position. 
    /// </summary>
    public void UpdateUnitPosition(Vector3Int currrentPosition, Vector3Int newPosition, Unit unit)
    {
        units.Remove(currrentPosition);
        units.Add(newPosition, unit);
    }

    public void RegisterUnit(Unit unit, Vector3Int position)
    {
        units.Add(position, unit);
    }

    public void UpdateUnitPosition(Unit unit, Vector3Int oldPosition, Vector3Int newPosition)
    {
        units.Remove(oldPosition);
        units.Add(newPosition, unit);
    }*/
}
