using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float lerpAlpha;

    private void LateUpdate() => transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpAlpha);
}