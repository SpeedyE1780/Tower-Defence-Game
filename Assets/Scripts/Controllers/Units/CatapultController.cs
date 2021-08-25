using UnityEngine;

public class CatapultController : RangedController
{
    [SerializeField] private Transform catapult;
    protected override Transform RotationTransform => catapult;

    protected override void SetProjectile()
    {
        CatapultProjectileController projectile = SpawnProjectile().GetComponent<CatapultProjectileController>();
        projectile.SetTarget(currentTarget, unitID.GetLayerMask());
        projectile.SetTargetPosition(currentTarget.transform.position);
    }
}