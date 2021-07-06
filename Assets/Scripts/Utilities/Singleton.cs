using System.Collections.Generic;
using UnityEngine;

using Type = System.Type;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; protected set; }

    protected static List<Type> instantiatedTypes;

    protected virtual void Awake()
    {
        if (instantiatedTypes == null)
            instantiatedTypes = new List<Type>();

        Type type = GetType();

        foreach (Type t in instantiatedTypes)
            Debug.Log($"{t} singleton in instantiated");

        if (Instance == null && !instantiatedTypes.Contains(type))
        {
            Instance = (T)this;
            instantiatedTypes.Add(type);
            Debug.Log($"Added singleton {type}");
        }
        else
        {
            Debug.Log($"Destroying {type}");
            Destroy(gameObject);
        }
    }
}