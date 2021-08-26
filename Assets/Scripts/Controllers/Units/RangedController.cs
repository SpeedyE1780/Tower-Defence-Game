using UnityEngine;

public class RangedController : UnitController
{
    [SerializeField] private Transform shootPoint;
    [Header("Visual Elements")]
    [SerializeField] private PoolID projectileID;
    [Range(0, 1)]
    [SerializeField] private float rotationSpeed;
    private Vector3 targetForward;

    //Transform that will rotate and follow the current target
    protected virtual Transform RotationTransform => transform;
    protected void Start() => targetForward = new Vector3();

    protected override void AttackTarget()
    {
        RotateTowardTarget();
        Shoot();
    }

    protected virtual void Shoot()
    {
        if (!CanAttack)
            return;

        ResetAttackCooldown();
        SetProjectile();
    }

    protected virtual void SetProjectile()
    {
        ProjectileController projectile = SpawnProjectile().GetComponent<ProjectileController>();
        projectile.SetTarget(currentTarget, unitMask);
    }

    protected GameObject SpawnProjectile()
    {
        Quaternion projectileRotation = Quaternion.LookRotation(shootPoint.forward);
        GameObject projectile = PoolManager.Instance.GetPooledObject(projectileID, shootPoint.position, projectileRotation);
        projectile.SetActive(true);
        return projectile;
    }

    protected virtual void RotateTowardTarget()
    {
        targetForward = currentTarget.transform.position - transform.position;
        RotationTransform.rotation = Quaternion.Slerp(RotationTransform.rotation, Quaternion.LookRotation(targetForward, Vector3.up), rotationSpeed);
    }
}