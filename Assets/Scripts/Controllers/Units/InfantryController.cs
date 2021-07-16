using UnityEngine;
using UnityEngine.AI;

public abstract class InfantryController : UnitController
{
    [SerializeField] private float attackRange;
    [Header("Movement Stats")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody agentBody;
    [SerializeField] protected Transform geometry;
    [SerializeField] protected float speed;
    private Quaternion rotation;

    private float DistanceToTarget => (currentTarget.transform.position - transform.position).sqrMagnitude;
    protected override bool HasIdleUpdate => true;

    protected virtual void Awake()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        attackRange *= attackRange;
        rotation = new Quaternion();
    }

    protected override void Update()
    {
        base.Update();
        if (unitAnimation.IsPlaying(AttackAnimation))
            return;

        if (agent.velocity.sqrMagnitude < 0.1f && !unitAnimation.IsPlaying(IdleAnimation))
            unitAnimation.Play(IdleAnimation);
        else if (agent.velocity.sqrMagnitude > 0.1f && !unitAnimation.IsPlaying(RunAnimation))
            unitAnimation.Play(RunAnimation);
    }

    protected override void SimulatePhysics(float deltaTime)
    {
        base.SimulatePhysics(deltaTime);
        agentBody.MovePosition(agent.nextPosition);
        geometry.position = agent.nextPosition;

        if (agent.velocity.sqrMagnitude > 0)
            SetLookRotation();
    }

    protected virtual void SetLookRotation()
    {
        if (agent.velocity.sqrMagnitude < 0.1f)
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
        unitAnimation.Play(AttackAnimation);
        currentTarget.TakeHit();
        ResetCooldown();
    }

    protected virtual bool TargetIsInRange() => DistanceToTarget <= attackRange;
    protected override void Idle() => throw new System.NotImplementedException();
}