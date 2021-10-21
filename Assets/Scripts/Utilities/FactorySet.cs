using System.Collections.Generic;
using UnityEngine;

//Set that contains a list of object with the same pool id
[CreateAssetMenu(menuName = "Pooling/Factory Set")]
public class FactorySet : ScriptableObject
{
    public PoolID SetID;
    public List<GameObject> SetItems;
    public GameObject GetRandomIten => SetItems[Random.Range(0, SetItems.Count)];
}
