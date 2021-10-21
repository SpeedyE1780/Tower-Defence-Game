using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float maxDistance;

    private void LateUpdate() => transform.position = Vector3.MoveTowards(transform.position, target.position + offset, maxDistance * Time.deltaTime);
}
