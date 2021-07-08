using UnityEngine;
using UnityEngine.AI;

public abstract class InfantryController : UnitController
{
    private static int SpeedParameter;

    [SerializeField] private float attackRange;
    [Header("Movement Stats")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private float speed;

    private float DistanceToTarget => (currentTarget.transform.position - transform.position).sqrMagnitude;
    protected override bool HasIdleUpdate => true;

    [RuntimeInitializeOnLoadMethod]
    private static void SetSpeedParameter()
    {
        SpeedParameter = Animator.StringToHash("Speed");
    }

    private void Awake()
    {
        agent.speed = speed;
        attackRange *= attackRange;
    }

    protected override void Update()
    {
        base.Update();
        anim.SetFloat(SpeedParameter, agent.velocity.sqrMagnitude);
    }

    protected override void AttackTarget()
    {
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
    protected override void Idle() => throw new System.NotImplementedException();
}