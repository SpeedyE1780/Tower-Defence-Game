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
        PoolManager.Instance.GetPooledObject(projectileID, shootPoint.position, Quaternion.LookRotation(shootPoint.forward)).SetActive(true);
        bulletCasing.Play();


        if (Random.Range(0, 1f) < hitChance)
            currentTarget.TakeHit();
    }

    protected virtual void Rotate()
    {
        targetForward = currentTarget.transform.position - transform.position;
        RotationTransform.rotation = Quaternion.Slerp(RotationTransform.rotation, Quaternion.LookRotation(targetForward, Vector3.up), rotationSpeed);
    }
}