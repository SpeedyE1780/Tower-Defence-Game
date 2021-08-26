using UnityEngine;

public class PoolParent : MonoBehaviour
{
    [SerializeField] PoolID poolID;

    public PoolID ParentID => poolID;
}