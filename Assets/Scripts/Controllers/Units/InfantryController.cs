using UnityEngine;
using UnityEngine.AI;

public class InfantryController : UnitController
{
    private const float IdleMovementThreshold = 0.1f;

    [Header("Attack Stats")]
    [SerializeField] private float attackRange;
    [SerializeField] protected int damage;
    [Header("Movement Stats")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected float speed;
    [SerializeField] protected bool instantKill;
    private Quaternion rotation;

    private float DistanceToTarget => (currentTarget.transform.position - transform.position).sqrMagnitude;

    #region UNITY MESSAGES

    protected override void Awake()
    {
        base.Awake();
        rotation = new Quaternion();
        DisableAgentAutoUpdate();
        agent.speed = speed;

        // Square attack range to compare it with the distance squared
        attackRange *= attackRange;
    }

    protected override void Update()
    {
        base.Update();
        UpdateTransform();
        UpdateAnimation();
    }

    #endregion

    #region UTILITY

    private void UpdateAnimation()
    {
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

    protected void UpdateTransform()
    {
        bool isMoving = agent.velocity.sqrMagnitude > IdleMovementThreshold;
        rotation = isMoving ? Quaternion.LookRotation(agent.velocity) : transform.rotation;
        transform.SetPositionAndRotation(agent.nextPosition, rotation);
    }

    private void DisableAgentAutoUpdate()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    #endregion

    #region ATTACK

    protected override void AttackTarget()
    {
        //Set destination to current target
        agent.SetDestination(currentTarget.transform.position);

        if (CanAttack() && TargetIsInRange())
            DamageTarget();
    }

    //Play attack animation and reset cooldown
    private void DamageTarget()
    {
        unitAnimation.Play(AttackAnimation);
        ApplyDamage();
        ResetAttackCooldown();
    }

    protected virtual void ApplyDamage() => currentTarget.TakeHit(damage, instantKill);
    protected virtual bool TargetIsInRange() => DistanceToTarget <= attackRange;

    #endregion
}
