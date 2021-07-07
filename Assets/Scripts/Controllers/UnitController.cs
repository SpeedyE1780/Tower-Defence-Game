using System.Collections;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    [Header("Detection Stats")]
    [SerializeField] private float detectionRaduis;
    [SerializeField] private float detectionCooldown;
    [SerializeField] private LayerMask detectionLayer;

    private UnitController currentTarget;

    protected virtual void Start() => StartCoroutine(LookForTarget());

    protected virtual void Update()
    {
        if (TargetFinder.IsTargetActive(currentTarget))
            AttackTarget();
        else
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

        currentTarget = TargetFinder.GetNearestTarget<UnitController>(enemies, transform.position);
    }

    protected abstract void AttackTarget();
    protected abstract void Idle();
}