using UnityEngine;

public class CatapultProjectileController : AOEProjectile
{
    [SerializeField] AnimationCurve catapultCurve;
    Vector3 targetPosition;
    float startY;

    protected override void OnEnable()
    {
        base.OnEnable();
        startY = transform.position.y;
    }

    protected override void Move()
    {
        base.Move();
        Vector3 position = transform.position;
        position.y = startY + catapultCurve.Evaluate(progress);
        transform.position = position;
    }

    //Set target position and get the max distance
    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
        targetPosition.y = 0;
    }
}