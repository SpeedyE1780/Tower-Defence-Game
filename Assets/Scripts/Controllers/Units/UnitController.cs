using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class UnitController : MonoBehaviour
{
    protected const string IdleAnimation = "Idle";
    protected const string ShootAnimation = "Shoot";
    protected const string RunAnimation = "Run";
    protected const string DeathAnimation = "Death";

    [SerializeField] protected PoolID poolID;

    [Header("Detection Stats")]
    [SerializeField] private int maxUnitsDetected;
    [SerializeField] private float detectionRaduis;
    [SerializeField] private float detectionCooldown;
    [SerializeField] private LayerMask detectionLayer;
    [Header("Attack Stats")]
    [SerializeField] protected float attackCooldown;

    protected float currentAttackCooldown;
    protected HealthController currentTarget;
    protected NativeArray<SpherecastCommand> commands;
    protected NativeArray<RaycastHit> hits;
    private float currentDetectionCooldown;

    protected virtual bool HasIdleUpdate => true;

    protected virtual void Awake()
    {
        commands = new NativeArray<SpherecastCommand>(1, Allocator.Persistent);
        hits = new NativeArray<RaycastHit>(1, Allocator.Persistent);
    }

    protected virtual void OnEnable()
    {
        currentTarget = null;
        currentAttackCooldown = attackCooldown;
        currentDetectionCooldown = detectionCooldown;
    }

    protected virtual void Update()
    {
        if (TargetFinder.IsTargetActive(currentTarget))
            AttackTarget();
        else if (HasIdleUpdate)
            Idle();

        float deltaTime = Time.deltaTime;
        currentAttackCooldown -= deltaTime;
        SimulatePhysics(deltaTime);
    }

    protected virtual void SimulatePhysics(float deltaTime)
    {
        if (currentDetectionCooldown < 0 && !TargetFinder.IsTargetActive(currentTarget))
            FindTarget();

        currentDetectionCooldown -= deltaTime;
    }

    protected virtual void FindTarget()
    {
        currentDetectionCooldown = detectionCooldown;
        currentTarget = null;
        commands[0] = new SpherecastCommand(transform.position + transform.forward * (detectionRaduis * -1.1f), detectionRaduis, transform.forward, 20, detectionLayer);
        SpherecastCommand.ScheduleBatch(commands, hits, 1).Complete();
        if (hits[0].transform != null)
            currentTarget = hits[0].transform.GetComponent<HealthController>();
    }

    protected virtual void OnDestroy()
    {
        hits.Dispose();
        commands.Dispose();
    }

    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(poolID, gameObject);
    protected virtual void ResetCooldown() => currentAttackCooldown = attackCooldown;
    protected abstract void AttackTarget();
    protected abstract void Idle();
}