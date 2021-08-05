using UnityEngine;

public class AllyInfantry : InfantryController
{
    protected Vector3 initialPosition;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.OnWaveEnded += ResetPosition;
        initialPosition = transform.position;
        UnitPlacementManager.RaiseUnitCount();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.OnWaveEnded -= ResetPosition;
        UnitPlacementManager.LowerUnitCount();
    }

    private void ResetPosition() => agent.SetDestination(initialPosition);

    protected override void Idle()
    {
        if (agent.destination != initialPosition)
            ResetPosition();
    }
}