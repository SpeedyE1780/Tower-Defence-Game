using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    private static Transform lookAtTarget;
    [RuntimeInitializeOnLoadMethod]
    public static void SetLookAt() => lookAtTarget = Camera.main.transform;
    Quaternion originalRotation;

    private void Awake() => originalRotation = transform.rotation;
    private void LateUpdate() => transform.rotation = lookAtTarget.rotation * originalRotation;
}