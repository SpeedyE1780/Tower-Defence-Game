using UnityEngine;

public class RangedController : UnitController
{
    [Header("Visual Elements")]
    [SerializeField] private PoolID projectileID;
    [SerializeField] private ParticleSystem bulletCasing;
    [Header("Shooting Stats")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown;
    [Range(0, 1)]
    [SerializeField] private float hitChance;
    [Range(0, 1)]
    [SerializeField] private float rotationSpeed;

    private float currentCooldown;
    private Vector3 targetForward;

    protected virtual Transform RotationTransform => transform;
    protected override bool HasIdleUpdate => false;

    protected void Start()
    {
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

    private void OnDrawGizmos()
    {
        Color color = Color.white;

        if (Application.isPlaying)
        {
            color = currentTarget == null ? Color.red : Color.green;
        }

        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, detectionRaduis);
    }
}