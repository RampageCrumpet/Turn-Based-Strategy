using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// This is a dataclass that holds all of the information needed to follow a path.
    /// </summary>
    public class Path 
    {
        public Stack<Vector2Int> nodes { get; protected set; }

        public MovementType PathType { get; protected set; }

        public int Cost { get; protected set; }

        //If the path can be followed. If no path exists or the target position is obstructed this will be false
        public bool isPathable { get; protected set;}


        public Path(Stack<Vector2Int> path, MovementType movementType, AStar pathfinder, bool canBeFollowed, Player issuingPlayer)
        {
            this.nodes = path;
            PathType = movementType;

            Cost = GetPathCost(path, movementType, pathfinder, issuingPlayer);

            isPathable = canBeFollowed;
        }

        //Extract the cost to move through a path.
        private int GetPathCost(Stack<Vector2Int> pathStack, MovementType movementType, AStar pathfinder, Player player)
        {
            if (nodes == null)
                return 0;

            int pathCost = 0;

            foreach (Vector2Int tilePosition in nodes)
            {
                pathCost += pathfinder.GetTileCost(tilePosition, movementType, player);
            }

            return pathCost;
        }

    }
}
