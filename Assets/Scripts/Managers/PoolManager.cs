using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private SerializableDictionaryBase<GameObject, PoolID> poolParents;
    private Dictionary<PoolID, Queue<GameObject>> pooledObjects;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    protected override void Awake()
    {
        base.Awake();
        pooledObjects = new Dictionary<PoolID, Queue<GameObject>>();
        PopulatePool();
        defaultPosition = Vector3.zero;
        defaultRotation = Quaternion.identity;
    }

    //Populate pool using the gameobject in the hierarchy
    private void PopulatePool()
    {
        foreach (Transform child in transform)
        {
            PoolID id = poolParents[child.gameObject];
            pooledObjects.Add(id, new Queue<GameObject>());
            foreach (Transform pooled in child)
                pooledObjects[id].Enqueue(pooled.gameObject);
        }
    }

    public GameObject GetPooledObject(PoolID id, Vector3 position = default, Quaternion rotation = default)
    {
        //Get pooled object
        if (pooledObjects.ContainsKey(id) && pooledObjects[id].Count > 0)
        {
            //Set position rotation
            if (position == default)
                position = defaultPosition;
            if (rotation == default)
                rotation = defaultRotation;

            GameObject poolItem = pooledObjects[id].Dequeue();
            poolItem.transform.SetPositionAndRotation(position, rotation);
            return poolItem;
        }
        else
        {
            //Instantiate new object
            GameObject factoryObject = FactoryManager.Instance.GetItem(id, position, rotation);
            return factoryObject;
        }
    }

    public T GetPooledObject<T>(PoolID id, Vector3 position = default, Quaternion rotation = default) where T : Component
    {
        return GetPooledObject(id, position, rotation).GetComponent<T>();
    }

    public void AddToPool(PoolID id, GameObject poolObject)
    {
        //Check that a similar object has been pooled
        if (pooledObjects.ContainsKey(id))
        {
            //Prevent pooling the same object twice
            if (!pooledObjects[id].Contains(poolObject))
                pooledObjects[id].Enqueue(poolObject);
        }
        else
        {
            //Create a new entry in the dictionary for this id
            pooledObjects.Add(id, new Queue<GameObject>(new[] { poolObject }));
        }

        //Disable gameobject and center it
        poolObject.SetActive(false);
        poolObject.transform.localPosition = Vector3.zero;
    }

    [ContextMenu("Populate Pool")]
    public void ScenePopulatePool()
    {
        FactoryManager factory = FindObjectOfType<FactoryManager>();

        foreach (PoolID id in factory.Catalog.Keys)
        {
            GameObject poolParent = new GameObject($"{id}");
            poolParent.transform.SetParent(transform);
        }
    }
}