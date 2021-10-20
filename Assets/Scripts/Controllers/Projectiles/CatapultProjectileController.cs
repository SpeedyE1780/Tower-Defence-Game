using UnityEngine;

public class CatapultProjectileController : AOEProjectile
{
    [SerializeField] private AnimationCurve catapultCurve;
    private Vector3 targetPosition;
    private float startY;

    public override void SetTarget(HealthController newTarget, int mask)
    {
        base.SetTarget(newTarget, mask);

        //Prevents updating movement target position
        targetDisabled = true;
    }

    protected override void ResetValues()
    {
        base.ResetValues();
        startY = transform.position.y;
    }

    protected override void Move()
    {
        base.Move();
        AddYOffset();
    }

    //Add offset to starting y based on catapult curve and movement progress
    private void AddYOffset()
    {
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
