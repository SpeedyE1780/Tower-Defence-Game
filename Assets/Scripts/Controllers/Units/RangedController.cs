using UnityEngine;

public class RangedController : UnitController
{
    [SerializeField] private Transform shootPoint;
    [SerializeField, Range(0, 1)] private float hitChance;
    [Header("Visual Elements")]
    [SerializeField] private PoolID projectileID;
    [SerializeField] private ParticleSystem bulletCasing;
    [Range(0, 1)]
    [SerializeField] private float rotationSpeed;
    private Vector3 targetForward;

    //Transform that will rotate and follow the current target
    protected virtual Transform RotationTransform => transform;
    protected override bool HasIdleUpdate => false;

    protected void Start() => targetForward = new Vector3();

    protected override void AttackTarget()
    {
        RotateTowardTarget();
        Shoot();
    }

    protected override void Idle() => throw new System.Exception();

    protected virtual void Shoot()
    {
        if (!CanAttack)
            return;

        ResetAttackCooldown();
        SpawnProjectile();

        //Play shoot animation
        bulletCasing.Play();

        //Randomly hit the target
        if (Random.Range(0, 1f) < hitChance)
            currentTarget.TakeHit();
    }

    private void SpawnProjectile()
    {
        Quaternion projectileRotation = Quaternion.LookRotation(shootPoint.forward);
        GameObject projectile = PoolManager.Instance.GetPooledObject(projectileID, shootPoint.position, projectileRotation);
        projectile.SetActive(true);
    }

    protected virtual void RotateTowardTarget()
    {
        targetForward = currentTarget.transform.position - transform.position;
        RotationTransform.rotation = Quaternion.Slerp(RotationTransform.rotation, Quaternion.LookRotation(targetForward, Vector3.up), rotationSpeed);
    }
}