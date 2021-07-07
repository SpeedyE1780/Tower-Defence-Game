using UnityEngine;

public class FactoryManager : Singleton<FactoryManager>
{
    public FactoryCatalog Catalog;

    public GameObject GetItem(PoolID id)
    {
        if (Catalog.ContainsKey(id))
        {
            FactoryCategory category = Catalog[id];
            GameObject item = category.GetRandomIten;
            return Instantiate(item);
        }
        else
        {
            Debug.LogError($"Missing key {id} from factory");
            return null;
        }
    }
}

[System.Serializable]
public class FactoryCatalog : SerializableDictionaryBase<PoolID, FactoryCategory> { }