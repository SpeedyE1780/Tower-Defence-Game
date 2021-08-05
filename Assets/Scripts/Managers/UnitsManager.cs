using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public static class UnitsManager
{
    private static Dictionary<bool, List<Transform>> activeUnits;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        activeUnits = new Dictionary<bool, List<Transform>>()
        {
            { false, new List<Transform>() },
            { true, new List<Transform>() }
        };
    }

    public static void AddUnit(bool isEnemy, Transform transform) => activeUnits[isEnemy].Add(transform);
    public static void RemoveUnit(bool isEnemy, Transform transform) => activeUnits[isEnemy].Remove(transform);
    public static Transform GetTarget(bool isEnemy) => activeUnits[isEnemy].GetRandomElement();
}