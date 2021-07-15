using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
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

    private void PopulatePool()
    {
        foreach (Transform child in transform)
        {
            System.Enum.TryParse(child.name, out PoolID id);
            pooledObjects.Add(id, new Queue<GameObject>());
            foreach (Transform pooled in child)
                pooledObjects[id].Enqueue(pooled.gameObject);
        }
    }

    public GameObject GetPooledObject(PoolID id, Vector3 position = default, Quaternion rotation = default)
    {
        if (pooledObjects.ContainsKey(id))
        {
            GameObject poolItem = pooledObjects[id].Dequeue();
            poolItem.transform.SetParent(null);

            if (position == default)
                position = defaultPosition;
            if (rotation == default)
                rotation = defaultRotation;

            poolItem.transform.SetPositionAndRotation(position, rotation);

            if (pooledObjects[id].Count <= 0)
                pooledObjects.Remove(id);

            return poolItem;
        }
        else
        {
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
        if (pooledObjects.ContainsKey(id) && !pooledObjects[id].Contains(poolObject))
            pooledObjects[id].Enqueue(poolObject);
        else
            pooledObjects.Add(id, new Queue<GameObject>(new[] { poolObject }));

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

public enum PoolID
{
    Enemy,
    Projectile,
    EnemyBoss,
    Tower,
    Infantry,
    Archer
}