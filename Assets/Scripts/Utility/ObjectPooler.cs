using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem 
{
    [SerializeField]
    [Tooltip("The game object we want to reserve in our pooler.")]
    public GameObject objectToPool;

    [SerializeField]
    [Tooltip("When enabled allows the object pooler to create new objects and add them to the pool if an object is requested and none are available.")]
    public bool canExpand = true;

    [SerializeField]
    [Tooltip("The starting number of objects to instantiate.")]
    public int amountToPool;
}


public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler objectPooler;

    private List<GameObject> pooledObjects;

    [SerializeField]
    [Tooltip("Reference containing the information needed to pool the object.")]
    private List<ObjectPoolItem> itemsToPool;



    void Awake()
    {
        //Enforce the singleton pattern
        if (objectPooler != null)
        {
            Debug.LogError("Multiple GameControllers detected in the scene.");
            Destroy(this);
        }
        else
        {
            objectPooler = this;
        }
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject newObject = (GameObject)Instantiate(item.objectToPool);
                newObject.SetActive(false);
                pooledObjects.Add(newObject);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeSelf && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach(ObjectPoolItem item in itemsToPool)
        {
            if(item.objectToPool.tag == tag)
            {
                if(item.canExpand)
                {
                    GameObject newObject = (GameObject)Instantiate(item.objectToPool);
                    newObject.SetActive(false);
                    pooledObjects.Add(newObject);
                    return newObject;
                }
            }
        }

        return null;
    }
}
