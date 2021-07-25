using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This tile handles all of the gameplay elements of a tile.
/// 
/// </summary>
/// 

public class GameTile : PathfindingTile
{
    public Unit unit = null;

    [SerializeField]
    [Tooltip("The number of defense dots this tile provides.")]
    int defense = 0;

    public GameTile(TileDefinition tileDefinition) : base(tileDefinition)
    {
        defense = tileDefinition.defense;
    }
}
