using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    public GameTile[,] tiles;

    //The tilemap we're drawing our information from.
    public Tilemap tilemap;

    [SerializeField]
    [Tooltip("A list of TileDef's used to define the properties of each tile type.")]
    List<TileDefinition> tileDefs = new List<TileDefinition>();


    private void Start()
    {
        Initialize();
        Unit.OnMove += MoveUnit;
    }

    //Setup the gameboard by loading information from the tilemap.
    private void Initialize()
    {
        //reset the tilemaps boundaries.
        tilemap.CompressBounds();

        BoundsInt tilemapBounds = tilemap.cellBounds;

        Vector3Int tileMapOrigin = tilemap.origin;


        tiles = new GameTile[tilemapBounds.size.x, tilemapBounds.size.y];

        for (int x = 0; x < tilemapBounds.size.x; x++)
        {
            for (int y = 0; y < tilemapBounds.size.y; y++)
            {
                //Convert from BoardCoordinates to TileMap coordinates
                Vector3Int tilePosition = new Vector3Int(x, y, 0) + tileMapOrigin;

                GameTile gametile = TileBaseToGameTile(tilemap.GetTile(tilePosition));

                tiles[x, y] = gametile;
            }
        }
    }

    /// <summary>
    /// Returns the GameTile stored at the current Board position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>

    public GameTile GetTile(Vector2Int position)
    {
        if (!Contains(position))
            throw new ArgumentOutOfRangeException(position.ToString(), "Position is outside of GameBoard bounds.");

        return tiles[position.x, position.y];
    }

    //Takes a world position and returns a gameboard position associated with it.
    public Vector2Int WorldToCell(Vector3 position)
    {
        //Convert from TileMap coordinates to GameBoard coordinates.
        Vector3Int tileMapPosition = tilemap.WorldToCell(position) - tilemap.origin;


        Vector2Int cellPosition = new Vector2Int(tileMapPosition.x, tileMapPosition.y);

        return cellPosition;

    }


    //Takes a gameboard position and returns the world position associated with it.
    public Vector3 CellToWorld(Vector2Int tilePosition)
    {
        Vector3Int tileMapPosition = new Vector3Int(tilePosition.x, tilePosition.y, 0) + tilemap.origin;
        return tilemap.CellToWorld(tileMapPosition);
    }

    GameTile TileBaseToGameTile(TileBase tileBase)
    {


        foreach (TileDefinition tileDef in tileDefs)
        {
            //Check to see if the tile defined in the TileBase is the same type of tile we're looking at now. 
            if (tileDef.tile == tileBase)
            {
                return new GameTile(tileDef);
            }
        }

        //We didn't find a tile that matches our tile.
        Debug.LogError("No TileDef found with a reference to Tile:" + tileBase.name);
        return null;

    }

    //Returns true if the target position is contained within the GameBoard, otherwise it returns false. 
    public bool Contains(Vector2Int position)
    {
        if (position.x < 0 || position.y < 0)
            return false;

        if (position.x >= tilemap.cellBounds.size.x || position.y >= tilemap.cellBounds.size.y)
            return false;

        return true;
    }

    //This method moves the unit to it's new spot on the gameboard. 
    private void MoveUnit(Unit unit, Vector2Int startPosition, Vector2Int targetPosition)
    {
        if (!Contains(startPosition) || !Contains(targetPosition))
        {
            Debug.LogError("Moving Unit to or from a position that is not on the game board.");
            return;
        }

        tiles[startPosition.x, startPosition.y].unit = null;
        tiles[targetPosition.x, targetPosition.y].unit = unit;

        Debug.Log("Unit position updated.");
    }




}
