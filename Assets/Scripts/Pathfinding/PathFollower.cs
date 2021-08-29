using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class PathFollower : MonoBehaviour
{
    [Tooltip("The speed at which the object follows the path given to it in UnityUnits/second.")]
    [SerializeField]
    float speed = 1;

    //The next position in the path we want to move towards. 
    Vector2Int targetCell;

    //The path for our object to follow.
    Stack<Vector2Int> path;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(path != null)
        {
            Vector3 worldTargetPosition = GameController.gameController.gameBoard.CellToWorld(targetCell);
            //Preserve the height of the gameobject.
            worldTargetPosition.z = this.transform.position.z;

            this.transform.position = Vector3.MoveTowards(transform.position, worldTargetPosition, speed*Time.deltaTime);

            if(Vector3.Distance(transform.position, worldTargetPosition) < 0.001f)
            {
                //Teleport us directly onto the target position
                transform.position = worldTargetPosition;

                //If we're not done following the path move to the next cell. 
                if(path.Count > 0)
                {
                    targetCell = path.Pop();
                }
                else
                {
                    path = null;
                }

            }
        }
    }

    public void FollowPath(Path path)
    {
        if (path == null)
        {
            Debug.LogWarning("PathFollower given empty path.");
            return;
        }
            
        this.path = path.nodes;
        targetCell  = this.path.Pop();
    }
}
