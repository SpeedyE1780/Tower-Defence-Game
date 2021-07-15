using UnityEngine;
using UnityEngine.AI;

public abstract class InfantryController : UnitController
{
    [SerializeField] private float attackRange;
    [Header("Movement Stats")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody agentBody;
    [SerializeField] protected float speed;
    private Quaternion rotation;

    private float DistanceToTarget => (currentTarget.transform.position - transform.position).sqrMagnitude;
    protected override bool HasIdleUpdate => true;

    protected override void Awake()
    {
        base.Awake();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        attackRange *= attackRange;
        rotation = new Quaternion();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        agentBody.MovePosition(agent.nextPosition);

        if (agent.velocity.sqrMagnitude > 0)
            SetLookRotation();
    }

    protected virtual void SetLookRotation()
    {
        if (agent.velocity.sqrMagnitude == 0)
            return;

        rotation.SetLookRotation(agent.velocity);
        agentBody.MoveRotation(rotation);
    }

    protected override void AttackTarget()
    {
        agent.SetDestination(currentTarget.transform.position);

        if (currentAttackCooldown < 0 && TargetIsInRange())
            DamageTarget();
    }

    private void DamageTarget()
    {
        currentTarget.TakeHit();
        ResetCooldown();
    }

    protected virtual bool TargetIsInRange() => DistanceToTarget <= attackRange;
    protected override void Idle() => throw new System.NotImplementedException();
}