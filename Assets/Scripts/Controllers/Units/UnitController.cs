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

    protected float currentCooldown;
    protected HealthController currentTarget;
    protected Collider[] detectedUnits;

    protected virtual bool HasIdleUpdate => true;

    protected virtual void Awake() => detectedUnits = new Collider[maxUnitsDetected];
    protected virtual void OnEnable() => StartCoroutine(LookForTarget());

    protected virtual void Update()
    {
        if (TargetFinder.IsTargetActive(currentTarget))
            AttackTarget();
        else if (HasIdleUpdate)
            Idle();

        currentCooldown -= Time.deltaTime;
    }

    protected virtual IEnumerator LookForTarget()
    {
        while (true)
        {
            if (!TargetFinder.IsTargetActive(currentTarget))
                FindTarget();

            yield return new WaitForSeconds(detectionCooldown);
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void FindTarget()
    {
        currentTarget = null;
        Physics.OverlapSphereNonAlloc(transform.position, detectionRaduis, detectedUnits, detectionLayer, QueryTriggerInteraction.Ignore);
        currentTarget = TargetFinder.GetNearestTarget(detectedUnits, transform.position, detectionRaduis * detectionRaduis);
    }

    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(poolID, gameObject);
    protected virtual void ResetCooldown() => currentCooldown = attackCooldown;
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