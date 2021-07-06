using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //Singleton
    private static PoolManager _instance;
    public static PoolManager Instance => _instance;


    private Dictionary<PoolID, Queue<GameObject>> pooledObjects; //Contains all the instantiated objects
    private Dictionary<PoolID, Transform> poolParents; //Contains are the parents of each object type

    //Initialize singleton and dictionaries
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        pooledObjects = new Dictionary<PoolID, Queue<GameObject>>();
        poolParents = new Dictionary<PoolID, Transform>();
    }

    public GameObject GetPooledObject(PoolID id)
    {
        //Check if dictionary contains id
        if (pooledObjects.ContainsKey(id))
        {
            //Get the first element in the queue and reset it's parent
            GameObject poolItem = pooledObjects[id].Dequeue();
            poolItem.transform.SetParent(null);
            poolItem.SetActive(true);

            //If queue is empty remove it from the dictionary
            if (pooledObjects[id].Count <= 0)
            {
                pooledObjects.Remove(id);
            }

            //Debug.Log($"Getting {poolItem.name} from pool");
            return poolItem;
        }

        //Get an instantiated element from the factory
        else
        {
            GameObject factoryObject = FactoryManager.Instance.GetItem(id);
            //Debug.Log($"Created {factoryObject.name} from Factory");
            return factoryObject;
        }
    }

    //Add an object to the pool
    public void AddToPool(PoolID id, GameObject poolObject)
    {
        //If id exists in dictionary enqueue object to queue
        if (pooledObjects.ContainsKey(id))
        {
            pooledObjects[id].Enqueue(poolObject);
        }

        //Create new dictionary entry with object as it's first element
        else
        {
            pooledObjects.Add(id, new Queue<GameObject>(new[] { poolObject }));
        }

        //Check if there exits a parent for this object
        if (!poolParents.ContainsKey(id))
            CreatePoolParent(id);

        poolObject.name = $"Pooled {id}";
        //Deactivate the object set it as a child and center it's position
        poolObject.SetActive(false);
        poolObject.transform.SetParent(poolParents[id]);
        poolObject.transform.localPosition = Vector3.zero;
    }

    //Create an empty parent to hold the pooled item of the same type
    void CreatePoolParent(PoolID id)
    {
        GameObject poolParent = new GameObject($"{id}");
        poolParent.transform.SetParent(transform);
        poolParents.Add(id, poolParent.transform);
    }
}

public enum PoolID
{
    Health,
    Coin,
    Stake,
    Laser,
    GarlicEffect,
    BulletEffect,
    StakeEffect,
    HealthEffect,
    CoinEffect,
    BulletPowerUp,
    ShockwavePowerUp,
    UICoin,
    HealthPickUp,
    Garlic,
    SilverBullet,
    None,
    TestingGarlic,
    TestingSilverBullet,
    TestingStake,
    TestingLaser,
    TestingHealth,
    TestingCoin
}