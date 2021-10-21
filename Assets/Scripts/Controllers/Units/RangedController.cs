using UnityEngine;

public class RangedController : UnitController
{
    [SerializeField] private Transform shootPoint;
    [Header("Visual Elements")]
    [SerializeField] private PoolID projectileID;
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

    protected virtual void RotateTowardTarget()
    {
        //Get desired forward
        targetForward = currentTarget.transform.position - transform.position;
        targetForward.y = 0;

        if (targetForward == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        RotationTransform.rotation = Quaternion.RotateTowards(RotationTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    protected virtual void Shoot()
    {
        if (!CanAttack())
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
}
