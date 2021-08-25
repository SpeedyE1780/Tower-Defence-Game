using UnityEngine;

public class CatapultProjectileController : AOEProjectile
{
    [SerializeField] AnimationCurve catapultCurve;
    Vector3 targetPosition;
    float startY;
    float maxDistance;

    private void OnEnable() => startY = transform.position.y;

    protected override void Move()
    {
        Vector3 position = transform.position;
        position.y = 0;

        //Move toward the target position and calculate the y position
        position = Vector3.MoveTowards(position, targetPosition, speed * Time.deltaTime);
        distance = (targetPosition - position).sqrMagnitude;
        position.y = startY + catapultCurve.Evaluate(distance / maxDistance);
        transform.forward = position - transform.position;
        transform.position = position;
    }

    //Set target position and get the max distance
    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
        targetPosition.y = 0;
        maxDistance = (targetPosition - transform.position).sqrMagnitude;
    }
}