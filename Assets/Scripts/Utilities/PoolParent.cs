using UnityEngine;

//Used to add all child objects in hierarchy to dictionary with poolid as key
public class PoolParent : MonoBehaviour
{
    [SerializeField] PoolID poolID;

    public PoolID ParentID => poolID;
}
