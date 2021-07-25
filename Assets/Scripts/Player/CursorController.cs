using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// This class moves the cursor to follow the players mouse snapping the selection indicator to the tilemap.
/// 
/// Alexander Van Oss 7/15/2021
/// </summary>
public class CursorController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The object that follows the mouse.")]
    GameObject cursorIndicator;

    GameBoard gameBoard;

    private void Start()
    {
        gameBoard = GameController.gameController.gameBoard;
    }


    // Update is called once per frame
    void Update()
    {
        //Snap the cursor indicator to the 
        cursorIndicator.transform.position = gameBoard.CellToWorld(GetMouseCell());
    }

    /// <summary>
    /// Returns the TileMap cell directly underneath the mouse.
    /// </summary>
    /// <returns>Returns a Vector3Int containing the cell position directly under the mouse.</returns>
    public Vector2Int GetMouseCell()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cellPosition = gameBoard.WorldToCell(mousePos);

        return cellPosition;
    }
}
