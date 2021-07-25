using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;


public class PathfindingTile 
{
   


    //The cost for a walking unit to cross this tile.
    int legCost = 1;

    //The cost for a wheeled unit to cross this tile.
    int wheelsCost = 1;

    //The cost for a naval unit to cross this tile.
    int sailCost = 1;

    //The cost for a hovering unit to cross this tile.
    int hoverCost = 1;

    //The cost for a treaded unit to cross this tile.
    int treadCost = 1;

    //The cost for a flying unit to cross this tile.
    int flyCost = 1;

    public PathfindingTile(TileDefinition tileDefinition)
    {
        legCost = tileDefinition.legCost;
        wheelsCost = tileDefinition.wheelsCost;

        sailCost = tileDefinition.sailCost;

        hoverCost = tileDefinition.hoverCost;

        treadCost = tileDefinition.treadCost;

        flyCost = tileDefinition.flyCost;
    }



    public int getPathfindingCosts(MovementType movementType)
    {
        switch (movementType)
        {
            case MovementType.Legs:
                return legCost;

            case MovementType.Wheels:
                return wheelsCost;

            case MovementType.Treads:
                return treadCost;

            case MovementType.Sail:
                return sailCost;

            case MovementType.Hover:
                return hoverCost;

            case MovementType.Fly:
                return flyCost;

            default:
                Debug.LogError(("Error {0} is not a movement type PathfindingTile.cs recognizes.", movementType.ToString()));
                return -1; //Assume the tile is impassible to this movement type. 
        }    
    }

}
