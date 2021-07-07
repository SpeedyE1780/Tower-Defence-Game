using UnityEngine;
using UnityEngine.AI;

public class InfantryController : UnitController
{
    [SerializeField] private float attackRange;
    [Header("Movement Stats")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private float speed;

    public Vector3 initialPosition;

    private float DistanceToTarget => (currentTarget.transform.position - transform.position).sqrMagnitude;
    protected override bool HasIdleUpdate => false;

    private void Awake()
    {
        agent.speed = speed;
        attackRange *= attackRange;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.OnWaveEnded += ResetPosition;
    }

    protected virtual void OnDisable()
    {
        Debug.Log("Disabling");
        EventManager.OnWaveEnded -= ResetPosition;
    }

    private void ResetPosition()
    {
        try
        {
            agent.SetDestination(initialPosition);
        }
        catch
        {
            Debug.LogError($"{name} state: {gameObject.activeInHierarchy}", gameObject);
        }
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