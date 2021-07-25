using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

public class RangeTest : MonoBehaviour
{
    [SerializeField]
    int maxRange = 5;

    [SerializeField]
    int minRange = 1;

    [SerializeField]
    GameObject inRangeIndicator;

    [SerializeField]
    RangeFinder rangeFinder;

    [SerializeField]
    MovementType movementType;

    [SerializeField]
    Tilemap tilemap;

    List<GameObject> displayObjects = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            
            foreach(GameObject gameObject in displayObjects)
            {
                GameObject.Destroy(gameObject);
            }

            Vector3Int mousePosition = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2Int mouseTilePosition = new Vector2Int(mousePosition.x, mousePosition.y);

            HashSet<Vector2Int> tileLocations = rangeFinder.GetTilesWithinRange(mouseTilePosition, (uint)minRange, (uint)maxRange, movementType);

            Debug.Log(tileLocations.Count);

            foreach (Vector2Int position in tileLocations)
            {
                displayObjects.Add(GameObject.Instantiate(inRangeIndicator, new Vector3(position.x, position.y, -10), inRangeIndicator.transform.rotation));
            }

        }
    }
}
