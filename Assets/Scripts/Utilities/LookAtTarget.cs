using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private static Transform lookAtTarget;
    [RuntimeInitializeOnLoadMethod]
    public static void SetLookAt() => lookAtTarget = Camera.main.transform;

    private void LateUpdate() => transform.rotation = lookAtTarget.rotation;
}