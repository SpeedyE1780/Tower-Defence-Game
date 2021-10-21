using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    //Get random element in list
    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list.Count == 0)
            throw new System.Exception("List is empty");

        return list[Random.Range(0, list.Count)];
    }
}
