using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;



namespace Pathfinding
{ 
    public class RangeFinder : MonoBehaviour
    {
        [SerializeField]
        GameBoard gameBoard;

        private Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();

        //Tiles 
        HashSet<Node> openList;
        HashSet<Node> closedList;


        //Tiles between the minimum and maximum range.
        HashSet<Vector2Int> goalTiles;

        //Tiles that are too close 



        Node currentNode;

   

        public HashSet<Vector2Int> GetTilesWithinRange(Vector2Int startingPosition,  uint min, uint max, MovementType movementType)
        {
            allNodes.Clear();
            openList = new HashSet<Node>();
            closedList = new HashSet<Node>();

            //The nodes we want to return. 
            goalTiles = new HashSet<Vector2Int>();

            currentNode = GetNode(startingPosition);
            openList.Add(currentNode);

            while (openList.Count > 0)
            {
                currentNode = openList.First();

                ExploreTile(currentNode, min, max, movementType);


                openList.Remove(currentNode);
                closedList.Add(currentNode);
            }

            return goalTiles;
        }

        private void ExploreTile(Node node, uint min, uint max, MovementType movementType)
        {
            List<Node> neighbors = FindNeighbors(node.position);

            foreach(Node neighbor in neighbors)
            {
                //The tile we're looking at is off of the game board.
                if (!gameBoard.Contains(neighbor.position))
                    break;

                if (GetTileCost(neighbor.position, movementType) == -1)
                {
                    neighbor.isWalkable = false;
                    break;
                }
                    

                //if the node is new or we have a better path to it update the nodes cost. 
                if((!openList.Contains(neighbor) && !closedList.Contains(neighbor)) 
                    || node.travelToCost + GetTileCost(neighbor.position, movementType) < neighbor.travelToCost)
                {
                    neighbor.travelToCost = node.travelToCost + GetTileCost(neighbor.position, movementType);
                }


                if(neighbor.travelToCost < min && goalTiles.Contains(neighbor.position))
                {
                    goalTiles.Remove(neighbor.position);
                }

                if(neighbor.travelToCost <= max && neighbor.travelToCost > min)
                {
                    goalTiles.Add(neighbor.position);
                }

                if (neighbor.travelToCost < max && !closedList.Contains(neighbor))
                    openList.Add(neighbor);
                }
        }



        private List<Node> FindNeighbors(Vector2Int parentPosition)
        {
            List<Node> neighbors = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //Only add the 4 cardinal directions to the neighbor list. 
                    if (Mathf.Abs(x) + Mathf.Abs(y) == 1)
                    {

                        Vector2Int neighborPosition = parentPosition + new Vector2Int(x, y);

                        Node neighbor = GetNode(neighborPosition);
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        private Node GetNode(Vector2Int position)
        {
            if (allNodes.ContainsKey(position))
            {
                return allNodes[position];
            }
            else
            {
                Node node = new Node(position);
                allNodes.Add(position, node);
                return node;
            }

        }

        //Gets the cost for moving across the tile. 
        private int GetTileCost(Vector2Int target, MovementType movementType)
        {
            int tileCost = (gameBoard.GetTile(target)).getPathfindingCosts(movementType);

            return tileCost;
        }
    }
}