using UnityEngine;

public class AOEProjectile : ProjectileController
{
    [SerializeField] protected float range;
    private Vector3 aoePosition;

    public override void SetTarget(HealthController newTarget, int mask)
    {
        base.SetTarget(newTarget, mask);
        aoePosition = newTarget.transform.position;
    }

    protected override void ApplyDamage()
    {
        AOEManager.Instance.ApplyAOEDamage(aoePosition, range, damage, unitMask);
    }
}