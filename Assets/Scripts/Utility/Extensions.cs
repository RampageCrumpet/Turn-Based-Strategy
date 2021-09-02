using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ExtensionMethods
{
    public static class Vector3Extensions
    {
        public static Vector3 GetGridsnapPosition(this Vector3 unsnappedPosition)
        {
            GameBoard gameBoard = GameController.gameController.gameBoard;
            Vector2Int gameBoardPosition = gameBoard.WorldToCell(unsnappedPosition);
            Vector3 snapPosition = gameBoard.CellToWorld(gameBoardPosition);

            //Preserve the Z position so we don't set a snap position ontop of the gameboard.
            snapPosition.z = unsnappedPosition.z ;

            return snapPosition;
        }
    }


}