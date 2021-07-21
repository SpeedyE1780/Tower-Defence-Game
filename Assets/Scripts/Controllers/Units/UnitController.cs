using System.Runtime.CompilerServices;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class UnitController : MonoBehaviour
{
    protected const string PlayerTag = "Player";
    protected const string IdleAnimation = "Idle";
    protected const string AttackAnimation = "Attack";
    protected const string RunAnimation = "Run";
    protected const string DeathAnimation = "Death";

    [SerializeField] protected PoolID poolID;
    [SerializeField] protected Animation unitAnimation;
    [Header("Detection Stats")]
    [SerializeField] private float detectionRaduis;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] protected SphereCollider detectionTrigger;
    [Header("Attack Stats")]
    [SerializeField] protected float attackCooldown;

    protected float currentAttackCooldown;
    protected HealthController currentTarget;

    protected virtual bool HasIdleUpdate => true;

    protected virtual void OnEnable()
    {
        detectionTrigger.radius = detectionRaduis;
        unitAnimation.Play(IdleAnimation);
        currentTarget = null;
        currentAttackCooldown = attackCooldown;
    }

    protected virtual void Update()
    {
        if (IsTargetActive())
        {
            AttackTarget();
        }
        else
        {
            currentTarget = null;
            if (HasIdleUpdate)
                Idle();
        }
        currentAttackCooldown -= Time.deltaTime;
        SimulatePhysics();
    }

    protected virtual void SimulatePhysics()
    {
        if (!IsTargetActive() && !detectionTrigger.enabled)
            detectionTrigger.enabled = true;
    }

    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(poolID, gameObject);
    protected virtual void ResetCooldown() => currentAttackCooldown = attackCooldown;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual bool IsTargetActive() => currentTarget != null && !currentTarget.IsDead && currentTarget.gameObject.activeInHierarchy;
    protected abstract void AttackTarget();
    protected abstract void Idle();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            currentTarget = other.GetComponent<HealthController>();
            detectionTrigger.enabled = false;
        }
    }
}