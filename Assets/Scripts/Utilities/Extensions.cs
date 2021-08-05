using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T GetRandomElement<T>(this List<T> list) where T : Component => list.Count == 0 ? null : list[Random.Range(0, list.Count)];
}