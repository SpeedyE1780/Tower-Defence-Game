using UnityEngine;

public class RangedController : UnitController
{
    [SerializeField] private Transform shootPoint;
    [Range(0, 1)]
    [SerializeField] private float hitChance;
    [Header("Visual Elements")]
    [SerializeField] private PoolID projectileID;
    [SerializeField] private ParticleSystem bulletCasing;
    [Range(0, 1)]
    [SerializeField] private float rotationSpeed;

    private Vector3 targetForward;

    protected virtual Transform RotationTransform => transform;
    protected override bool HasIdleUpdate => false;

    protected void Start() => targetForward = new Vector3();

    protected override void AttackTarget()
    {
        Rotate();
        Shoot();
    }

    protected override void Idle() => throw new System.Exception();

    protected virtual void Shoot()
    {
        if (currentAttackCooldown > 0)
            return;

        ResetCooldown();
        PlayShootAnimation();
        SpawnProjectile();
        bulletCasing.Play();


        if (Random.Range(0, 1f) < hitChance)
            currentTarget.TakeHit();
    }

    protected virtual void PlayShootAnimation()
    {
        if (unitAnimation != null && unitAnimation.gameObject.activeInHierarchy)
            unitAnimation.Play(ShootAnimation);
    }

    protected virtual void Rotate()
    {
        targetForward = currentTarget.transform.position - transform.position;
        RotationTransform.rotation = Quaternion.Slerp(RotationTransform.rotation, Quaternion.LookRotation(targetForward, Vector3.up), rotationSpeed);
    }

    private void SpawnProjectile()
    {
        GameObject projectile = PoolManager.Instance.GetPooledObject(projectileID);
        projectile.transform.position = shootPoint.position;
        projectile.transform.forward = shootPoint.forward;
    }
}