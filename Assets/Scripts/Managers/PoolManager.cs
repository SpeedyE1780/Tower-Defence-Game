using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject pooledObject;

    public GameObject Spawn()
    {
        if (transform.childCount > 0)
        {
            GameObject spawned = transform.GetChild(0).gameObject;
            spawned.transform.SetParent(null);
            return spawned;
        }
        else
        {
            return Instantiate(pooledObject);
        }
    }

    public void Pool(GameObject pooled)
    {
        pooled.SetActive(false);
        pooled.transform.SetParent(transform);
    }
}