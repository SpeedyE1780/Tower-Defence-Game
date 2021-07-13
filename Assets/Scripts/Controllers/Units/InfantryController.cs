using UnityEngine;
using UnityEngine.AI;

public abstract class InfantryController : UnitController
{
    [SerializeField] private float attackRange;
    [Header("Movement Stats")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected float speed;

    private float DistanceToTarget => (currentTarget.transform.position - transform.position).sqrMagnitude;
    protected override bool HasIdleUpdate => true;

    protected override void Awake()
    {
        base.Awake();
        agent.speed = speed;
        attackRange *= attackRange;
    }

    protected override void Update()
    {
        base.Update();
        if (unitAnimation != null && unitAnimation.gameObject.activeInHierarchy)
            if (agent.velocity.sqrMagnitude > 0 && unitAnimation.IsPlaying(IdleAnimation))
                unitAnimation.Play(RunAnimation);
            else if (agent.velocity.sqrMagnitude == 0 && unitAnimation.IsPlaying(RunAnimation))
                unitAnimation.Play(IdleAnimation);
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