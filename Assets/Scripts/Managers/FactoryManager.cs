using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : Singleton<FactoryManager>
{
    [SerializeField] List<FactorySet> activeSets;
    public Dictionary<PoolID, FactorySet> factorySets;

    private void Start()
    {
        factorySets = new Dictionary<PoolID, FactorySet>();

        foreach (FactorySet set in activeSets)
            factorySets.Add(set.SetID, set);
    }

    public GameObject GetItem(PoolID id, Vector3 position, Quaternion rotation)
    {
        if (factorySets.ContainsKey(id))
        {
            FactorySet category = factorySets[id];
            GameObject item = category.GetRandomIten;
            return Instantiate(item, position, rotation);
        }
        else
        {
            Debug.LogError($"Missing key {id} from factory");
            return null;
        }
    }
}