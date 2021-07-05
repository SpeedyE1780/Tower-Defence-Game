using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetFinder
{
    public static bool IsTargetActive(EnemyController target) => target != null && target.gameObject.activeInHierarchy;

    public static Transform GetNearestTarget(Collider[] targetsInRange, Vector3 center)
    {
        float maxDistance = Mathf.Infinity;
        Transform target = targetsInRange[0].transform;
        Vector3 temp;

        for (int enemy = 1; enemy < targetsInRange.Length; enemy++)
        {
            temp = center - targetsInRange[enemy].transform.position;
            if (temp.sqrMagnitude < maxDistance)
            {
                target = targetsInRange[enemy].transform;
                maxDistance = temp.sqrMagnitude;
            }
        }

        return target;
    }
}