using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private static Transform lookAtTarget;

    private void Awake()
    {
        if (lookAtTarget == null)
            lookAtTarget = Camera.main.transform;
    }

    private void LateUpdate() => transform.rotation = lookAtTarget.rotation;
}