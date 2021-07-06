using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FactoryCategory")]
public class FactoryCategory : ScriptableObject
{
    public List<GameObject> CategoryItems;
    public GameObject GetRandomIten => CategoryItems[Random.Range(0, CategoryItems.Count)];
}