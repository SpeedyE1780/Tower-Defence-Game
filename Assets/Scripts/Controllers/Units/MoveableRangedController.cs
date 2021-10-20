using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveableRangedController : RangedController
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float distanceThreshold;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(ActivateAgent());
    }

    protected override void Update()
    {
        base.Update();
        SetDestination();
    }

    //Wait one frame for agent to update itself and avoid agent isn't on navmesh error
    private IEnumerator ActivateAgent()
    {
        yield return null;
        agent.isStopped = false;
    }

    private void SetDestination()
    {
        if (currentTarget == null)
            return;

        //Set destination to target and stop once close enough
        agent.SetDestination(currentTarget.Position);
        agent.isStopped = agent.remainingDistance < distanceThreshold;
    }

    protected override bool CanAttack() => base.CanAttack() && agent.isStopped;
}
