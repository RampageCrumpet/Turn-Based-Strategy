using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileDefinition", menuName ="Tiles/TileDefinition", order = 1)]
public class TileDefinition : ScriptableObject
{
    [Header("Game Values")]
    [SerializeField]
    [Tooltip("The tile this definition is defining.")]
    public Tile tile;


    [Space(10)]


    [Header("Pathfinding Costs")]
    [SerializeField]
    [Tooltip("The cost for a walking unit to cross this tile.")]
    public int legCost = 1;

    [SerializeField]
    [Tooltip("The cost for a wheeled unit to cross this tile.")]
    public int wheelsCost = 1;

    [SerializeField]
    [Tooltip("The cost for a naval unit to cross this tile.")]
    public int sailCost = 1;

    [SerializeField]
    [Tooltip("The cost for a hovering unit to cross this tile.")]
    public int hoverCost = 1;

    [SerializeField]
    [Tooltip("The cost for a treaded unit to cross this tile.")]
    public int treadCost = 1;

    [SerializeField]
    [Tooltip("The cost for a flying unit to cross this tile.")]
    public int flyCost = 1;
  

    [Space(10)]


    [Header("Game Values")]
    [SerializeField]
    [Tooltip("The number of defense dots this tile provides.")]
    public int defense = 0;
}
