using UnityEngine;
using UnityEngine.AI;

public class InfantryController : UnitController
{
    private const float IdleMovementThreshold = 0.1f;

    [SerializeField] private float attackRange;
    [Header("Movement Stats")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody agentBody;
    [SerializeField] protected Transform geometry;
    [SerializeField] protected float speed;
    [SerializeField] protected bool instantKill;
    private Quaternion rotation;

    private float DistanceToTarget => (currentTarget.transform.position - transform.position).sqrMagnitude;

    protected virtual void Awake()
    {
        rotation = new Quaternion();
        DisableAgentAutoUpdate();
        agent.speed = speed;

        // Square attack range to compare it with the distance squared
        attackRange *= attackRange;
    }

    protected override void Update()
    {
        base.Update();
        SimulatePhysics();

        if (unitAnimation.IsPlaying(AttackAnimation))
            return;

        SetMovementAnimation();
    }

    private void SetMovementAnimation()
    {
        if (agent.velocity.sqrMagnitude < IdleMovementThreshold && !unitAnimation.IsPlaying(IdleAnimation))
            unitAnimation.Play(IdleAnimation);
        else if (agent.velocity.sqrMagnitude > IdleMovementThreshold && !unitAnimation.IsPlaying(RunAnimation))
            unitAnimation.Play(RunAnimation);
    }

    protected void SimulatePhysics()
    {
        //Update navmesh position and geometry position
        agentBody.MovePosition(agent.nextPosition);
        geometry.position = agent.nextPosition;

        if (agent.velocity.sqrMagnitude > IdleMovementThreshold)
            SetLookRotation();
    }

    //Update navmesh rotation
    protected virtual void SetLookRotation()
    {
        rotation.SetLookRotation(agent.velocity);
        agentBody.MoveRotation(rotation);
    }

    protected override void AttackTarget()
    {
        //Set destination to current target
        agent.SetDestination(currentTarget.transform.position);

        if (CanAttack && TargetIsInRange())
            DamageTarget();
    }

    private void DamageTarget()
    {
        unitAnimation.Play(AttackAnimation);
        currentTarget.TakeHit(1, instantKill);
        ResetAttackCooldown();
    }

    private void DisableAgentAutoUpdate()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected virtual bool TargetIsInRange() => DistanceToTarget <= attackRange;
}