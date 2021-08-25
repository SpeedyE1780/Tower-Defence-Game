using UnityEngine;

public class AOEProjectile : ProjectileController
{
    [SerializeField] protected float range;

    protected override void ApplyDamage()
    {
        AOEManager.Instance.ApplyAOEDamage(transform.position, range, damage, unitMask);
    }
}