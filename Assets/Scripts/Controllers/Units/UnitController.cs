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
    [SerializeField] private float detectionRaduis;
    [SerializeField] private float detectionCooldown;
    [SerializeField] private LayerMask detectionLayer;
    [Header("Attack Stats")]
    [SerializeField] protected float attackCooldown;

    protected int commandIndex;
    protected float currentAttackCooldown;
    protected HealthController currentTarget;
    private float currentDetectionCooldown;

    protected virtual bool HasIdleUpdate => true;

    protected virtual void Awake()
    {
        commandIndex = UnitsManager.Instance.AddCommandToList(transform.position, detectionRaduis, transform.forward, detectionLayer);
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

        UnitsManager.Instance.UpdateCommand(commandIndex, transform.position, transform.forward);
        currentDetectionCooldown -= deltaTime;
    }

    protected virtual void FindTarget()
    {
        currentDetectionCooldown = detectionCooldown;
        currentTarget = UnitsManager.Instance.GetTarget(commandIndex);
    }

    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(poolID, gameObject);
    protected virtual void ResetCooldown() => currentAttackCooldown = attackCooldown;
    protected abstract void AttackTarget();
    protected abstract void Idle();
}