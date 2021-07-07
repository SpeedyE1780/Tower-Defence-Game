using UnityEngine;
using UnityEngine.AI;

public class InfantryController : UnitController
{
    [SerializeField] private float attackRange;
    [Header("Movement Stats")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private float speed;

    private float DistanceToTarget => (currentTarget.transform.position - transform.position).sqrMagnitude;

    private void Awake()
    {
        agent.speed = speed;
        attackRange *= attackRange;
    }

    protected override void AttackTarget()
    {
        agent.isStopped = false;
        agent.SetDestination(currentTarget.transform.position);

        if (currentCooldown < 0 && TargetIsInRange())
            DamageTarget();
    }

    private void DamageTarget()
    {
        currentTarget.TakeHit();
        ResetCooldown();
    }

    protected virtual bool TargetIsInRange() => DistanceToTarget <= attackRange;
    protected override void Idle() => agent.isStopped = true;
}