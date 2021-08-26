using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pooling/Factory Set")]
public class FactorySet : ScriptableObject
{
    public PoolID SetID;
    public List<GameObject> SetItems;
    public GameObject GetRandomIten => SetItems[Random.Range(0, SetItems.Count)];
}