using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using ExtensionMethods;

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

    void Start()
    {
        gameBoard = GameController.gameController.gameBoard;
    }


    // Update is called once per frame
    void Update()
    {
        //Snap the cursor indicator to the 
        Vector3 snappedWorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).GetGridsnapPosition();
        snappedWorldMousePosition.z = -1;
        cursorIndicator.transform.position = snappedWorldMousePosition;

    }


}
