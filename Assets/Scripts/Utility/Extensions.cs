using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    public static class Vector3Extensions
     {
        public static Vector3 GetGridsnapPosition(this Vector3 unsnappedPosition)
        {
            GameBoard gameBoard = GameController.gameController.gameBoard;
            Vector2Int gameBoardPosition = gameBoard.WorldToCell(unsnappedPosition);

            return gameBoard.CellToWorld(gameBoardPosition);
        }
    }
}