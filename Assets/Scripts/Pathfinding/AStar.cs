using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
    class Node
    {
        public Node(Vector2Int nodePosition)
        {
            position = nodePosition;
        }

        public Vector2Int position;
        public Node parent;

        //The actual cost to get to this node. 
        public int travelToCost = 0;

        //The estimated cost to get from this node to the target.
        public int heuristicCost = 0;

        //The cost of getting from the start position to this node plus the estimated cost to get to the end position.
        public int estimatedTotalCost = 0;

        public bool isWalkable = true;
    }

    public enum MovementType {Legs, Wheels, Treads, Hover, Sail, Fly, TrueDistance}

    public class AStar : MonoBehaviour
    {
        private GameBoard gameBoard;

        private HashSet<Node> openList;
        private HashSet<Node> closedList;

        Stack<Vector2Int> path;

        private Node currentNode;

        //Contains all of the nodes. Whenever we create a new node we need to add it to the dictionary. 
        private Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();

        Vector2Int startPosition;
        Vector2Int targetPosition;


        private void Start()
        {
            gameBoard = GameController.gameController.gameBoard;
        }

        /// <summary>
        /// Generates a path between two GameBoard positions and returns it. 
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="targetPosition"></param>
        /// <param name="movementType"></param>
        /// <returns></returns>
        public Stack<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int targetPosition, MovementType movementType)
        {
            openList = new HashSet<Node>();
            closedList = new HashSet<Node>();
            allNodes.Clear();

            path = null;

            this.startPosition = startPosition;
            this.targetPosition = targetPosition;

            currentNode = GetNode((Vector2Int)this.startPosition);
            openList.Add(currentNode);


            if(!gameBoard.Contains(startPosition) || !gameBoard.Contains(targetPosition))
            {
                Debug.LogWarning("The gameBoard does not contain the start or end position of the path.");
                return null;
            }


            CreatePath(movementType);

            return path;
        }

        //Run each step of A*
        private void CreatePath(MovementType movementType)
        {
            while(openList.Count > 0 && path == null)
            {
                List<Node> neighbors = FindNeighbors(currentNode.position, movementType);
                
                ExamineNeighbors(neighbors, currentNode, movementType);

                UpdateCurrentTile(currentNode);

                path = GeneratePath(currentNode);
            }

        }


        private Node GetNode(Vector2Int position)
        {
            if(allNodes.ContainsKey(position))
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

        private List<Node> FindNeighbors(Vector2Int parentPosition, MovementType movementType)
        {
            List<Node> neighbors = new List<Node>();

            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    //Only add the 4 cardinal directions to the neighbor list. 
                    if(Mathf.Abs(x) + Mathf.Abs(y) == 1)
                    {
                        Vector2Int neighborPosition = parentPosition + new Vector2Int(x, y);

                        if(gameBoard.Contains(neighborPosition))
                        {
                            Node neighbor = GetNode(neighborPosition);
                            neighbors.Add(neighbor);
                        }
                    }
                }
            }

            return neighbors;
        }

        private void SetTileCost(Node parent, Node node, MovementType movementType)
        {
            node.parent = parent;

            if(GetTileCost(node.position, movementType) < 0)
            {
                node.isWalkable = false;
            }

            node.travelToCost = parent.travelToCost + GetTileCost(node.position, movementType);
            node.heuristicCost = Mathf.Abs(node.position.x - targetPosition.x) + Mathf.Abs(node.position.y + targetPosition.y);
            node.estimatedTotalCost = node.travelToCost + node.heuristicCost;
        }

        //Gets the cost for moving across the tile. 
        private int GetTileCost(Vector2Int target, MovementType movementType)
        {
            int tileCost = gameBoard.GetTile(target).getPathfindingCosts(movementType);
            return tileCost;
        }

        private void ExamineNeighbors(List<Node> neighbors, Node current, MovementType movementType)
        {
            for (int x = 0; x < neighbors.Count; x++)
            {
                if(openList.Contains(neighbors[x]))
                {
                    if(current.travelToCost + GetTileCost(neighbors[x].position, movementType) < neighbors[x].travelToCost)
                    {
                        SetTileCost(current, neighbors[x], movementType);
                    }
                    
                }
                else if(!closedList.Contains(neighbors[x]))
                {
                    SetTileCost(current, neighbors[x], movementType);

                    //Using Negative 1 as a flag to see if a tile is walkable. If it's not we add it to the closed list. 
                    if(neighbors[x].isWalkable)
                    {
                        openList.Add(neighbors[x]);
                    }
                    else
                    {
                        closedList.Add(neighbors[x]);
                    }
                }
            }
        }


        private void UpdateCurrentTile(Node targetNode)
        {
            openList.Remove(targetNode);
            closedList.Add(targetNode);

            if(openList.Count > 0)
            {
                currentNode = openList.OrderBy(x => x.estimatedTotalCost).First();
            }
        }

        //See if we've found the destination, if so we want to follow it's path back to the start and return that path. 
        private Stack<Vector2Int> GeneratePath(Node node)
        {
            if(node.position == targetPosition)
            {
                Debug.Log("Found the target Position");
                Stack<Vector2Int> path = new Stack<Vector2Int>();

                //Create the path by following the nodes back to the start.
                while(node.position != startPosition)
                {
                    path.Push(node.position);
                    node = node.parent;
                }

                Debug.Log("Found a path.");

                return path;
            }

            return null;
        }

        //Extract the cost to move through a path.
        public int GetPathCost(Stack<Vector2Int> pathStack, MovementType movementType)
        {
            if (path == null)
                return 0;

            int pathCost = 0;

            foreach (Vector2Int tilePosition in path)
            {
                pathCost += GetTileCost(tilePosition, movementType);
            }

            return pathCost;
        }

    }



}
