using UnityEngine;
using UnityEngine.AI;

public class MoveableRangedController : RangedController
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float distanceThreshold;

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.isStopped = false;
    }

    protected override void Update()
    {
        base.Update();
        SetDestination();
    }

    private void SetDestination()
    {
        if (currentTarget == null)
            return;

        //Set destination to target and stop once close enough
        agent.SetDestination(currentTarget.Position);
        float distance = (transform.position - currentTarget.Position).magnitude;
        agent.isStopped = distance < distanceThreshold;
    }

    protected override bool CanAttack() => base.CanAttack() && agent.isStopped;
}