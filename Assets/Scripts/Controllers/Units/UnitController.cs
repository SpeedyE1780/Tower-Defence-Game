using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class UnitController : MonoBehaviour
{
    [SerializeField] protected Animator anim;
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
    private Collider[] detectedUnits;
    private float currentDetectionCooldown;

    protected virtual bool HasIdleUpdate => true;

    protected virtual void Awake() => detectedUnits = new Collider[maxUnitsDetected];

    protected virtual void Update()
    {
        if (TargetFinder.IsTargetActive(currentTarget))
            AttackTarget();
        else if (HasIdleUpdate)
            Idle();

        currentAttackCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (currentDetectionCooldown < 0 && !TargetFinder.IsTargetActive(currentTarget))
            FindTarget();

        currentDetectionCooldown -= Time.fixedDeltaTime;
    }

    protected virtual void FindTarget()
    {
        currentDetectionCooldown = detectionCooldown;
        currentTarget = null;
        Physics.OverlapSphereNonAlloc(transform.position, detectionRaduis, detectedUnits, detectionLayer, QueryTriggerInteraction.Ignore);
        currentTarget = TargetFinder.GetNearestTarget(detectedUnits, transform.position, detectionRaduis * detectionRaduis);
    }

    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(poolID, gameObject);
    protected virtual void ResetCooldown() => currentAttackCooldown = attackCooldown;
    protected abstract void AttackTarget();
    protected abstract void Idle();

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