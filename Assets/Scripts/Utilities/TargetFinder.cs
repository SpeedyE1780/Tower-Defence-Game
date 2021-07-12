using UnityEngine;

public static class TargetFinder
{
    public static bool IsTargetActive(HealthController target) => target != null && !target.IsDead && target.gameObject.activeInHierarchy;

    public static HealthController GetNearestTarget(Collider[] targetsInRange, Vector3 center, float maxDistance)
    {
        Transform target = null;
        Vector3 temp;

        for (int enemy = 0; enemy < targetsInRange.Length; enemy++)
        {
            Collider currentTarget = targetsInRange[enemy];

            if (currentTarget == null || !currentTarget.gameObject.activeSelf)
                continue;

            temp = center - currentTarget.transform.position;
            if (temp.sqrMagnitude < maxDistance)
            {
                target = currentTarget.transform;
                maxDistance = temp.sqrMagnitude;
            }
        }

        return target?.GetComponent<HealthController>();
    }
}