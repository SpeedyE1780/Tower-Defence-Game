using UnityEngine;

public class RangedController : UnitController
{
    [Header("Visual Elements")]
    [SerializeField] private PoolID projectileID;
    [SerializeField] private ParticleSystem bulletCasing;
    [Header("Shooting Stats")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float hitChance;
    [Range(0, 1)]
    [SerializeField] private float rotationSpeed;
    [Range(0, 1)]

    private float currentCooldown;
    private Vector3 targetForward;

    protected virtual Transform RotationTransform => transform;
    protected override bool HasIdleUpdate => false;

    protected override void Start()
    {
        base.Start();
        targetForward = new Vector3();
    }

    protected override void Update()
    {
        base.Update();
        currentCooldown -= Time.deltaTime;
    }

    protected override void AttackTarget()
    {
        Rotate();
        Shoot();
    }

    protected override void Idle() => throw new System.Exception();

    protected virtual void Shoot()
    {
        if (currentCooldown > 0)
            return;

        SpawnProjectile();
        currentCooldown = shootCooldown;
        bulletCasing.Play();

        if (Random.Range(0, 1f) < hitChance)
            currentTarget.TakeHit();
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