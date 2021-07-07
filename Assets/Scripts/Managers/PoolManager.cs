using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<PoolID, Queue<GameObject>> pooledObjects;
    private Dictionary<PoolID, Transform> poolParents;

    protected override void Awake()
    {
        base.Awake();
        pooledObjects = new Dictionary<PoolID, Queue<GameObject>>();
        poolParents = new Dictionary<PoolID, Transform>();
        PopulatePool();
    }

    private void PopulatePool()
    {
        foreach (Transform child in transform)
        {
            System.Enum.TryParse(child.name, out PoolID id);
            poolParents.Add(id, child);
            pooledObjects.Add(id, new Queue<GameObject>());
            foreach (Transform pooled in child)
                pooledObjects[id].Enqueue(pooled.gameObject);

            Debug.Log($"{id} initialized with {pooledObjects[id].Count} objects");
        }
    }

    public GameObject GetPooledObject(PoolID id)
    {
        if (pooledObjects.ContainsKey(id))
        {
            GameObject poolItem = pooledObjects[id].Dequeue();
            poolItem.transform.SetParent(null);
            poolItem.SetActive(true);

            if (pooledObjects[id].Count <= 0)
                pooledObjects.Remove(id);

            return poolItem;
        }
        else
        {
            GameObject factoryObject = FactoryManager.Instance.GetItem(id);
            return factoryObject;
        }
    }

    public T GetPooledObject<T>(PoolID id) where T : Component
    {
        return GetPooledObject(id).GetComponent<T>();
    }

    public void AddToPool(PoolID id, GameObject poolObject)
    {
        if (pooledObjects.ContainsKey(id) && !pooledObjects[id].Contains(poolObject))
            pooledObjects[id].Enqueue(poolObject);
        else
            pooledObjects.Add(id, new Queue<GameObject>(new[] { poolObject }));

        if (!poolParents.ContainsKey(id))
            CreatePoolParent(id);

        poolObject.name = $"Pooled {id}";
        poolObject.SetActive(false);
        poolObject.transform.SetParent(poolParents[id]);
        poolObject.transform.localPosition = Vector3.zero;
    }

    void CreatePoolParent(PoolID id)
    {
        GameObject poolParent = new GameObject($"{id}");
        poolParent.transform.SetParent(transform);
        poolParents.Add(id, poolParent.transform);
    }

    [ContextMenu("Populate Pool")]
    public void ScenePopulatePool()
    {
        FactoryManager factory = FindObjectOfType<FactoryManager>();

        foreach (PoolID id in factory.Catalog.Keys)
        {
            foreach (GameObject gameObject in factory.Catalog[id].CategoryItems)
            {
                GameObject g = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
                GameObject poolParent = new GameObject($"{id}");
                poolParent.transform.SetParent(transform);
                g.SetActive(false);
                g.transform.SetParent(transform.GetChild(transform.childCount - 1));
            }
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