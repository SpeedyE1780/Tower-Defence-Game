using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthController))]
public abstract class UnitController : MonoBehaviour
{
    [Header("Detection Stats")]
    [SerializeField] protected float detectionRaduis;
    [SerializeField] protected float detectionCooldown;
    [SerializeField] protected LayerMask detectionLayer;

    protected HealthController currentTarget;
    protected virtual bool HasIdleUpdate => true;

    protected virtual void OnEnable() => StartCoroutine(LookForTarget());

    protected virtual void Update()
    {
        if (TargetFinder.IsTargetActive(currentTarget))
            AttackTarget();
        else if (HasIdleUpdate)
            Idle();
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
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRaduis, detectionLayer, QueryTriggerInteraction.Ignore);

        if (enemies.Length == 0)
            return;

        currentTarget = TargetFinder.GetNearestTarget<HealthController>(enemies, transform.position);
    }

    protected abstract void AttackTarget();
    protected abstract void Idle();
}