using UnityEngine;

public class FactoryManager : Singleton<FactoryManager>
{
    public FactoryCatalog Catalog;

    public GameObject GetItem(PoolID id, Vector3 position, Quaternion rotation)
    {
        if (Catalog.ContainsKey(id))
        {
            FactorySet category = Catalog[id];
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

[System.Serializable]
public class FactoryCatalog : SerializableDictionaryBase<PoolID, FactorySet> { }