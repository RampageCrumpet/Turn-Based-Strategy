using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ClickMoveTest : MonoBehaviour
{
    [SerializeField]
    PathFollower pathFollower;

    [SerializeField]
    AStar astar;
 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int targetPosition = GameController.gameController.gameBoard.WorldToCell(mousePosition);
            Vector2Int startPosition = GameController.gameController.gameBoard.WorldToCell(this.transform.position);

            Debug.Log("target " + targetPosition + "    start " + startPosition);

            Stack <Vector2Int> path = astar.FindPath(startPosition, targetPosition, MovementType.Legs);

            pathFollower.FollowPath(path);

        }
    }
}
