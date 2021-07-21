using UnityEngine;

public class AllyInfantry : InfantryController
{
    protected Vector3 initialPosition;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.OnWaveEnded += ResetPosition;
        initialPosition = transform.position;
    }

    private void OnDisable()
    {
        EventManager.OnWaveEnded -= ResetPosition;
    }
    private void ResetPosition() => agent.SetDestination(initialPosition);

    protected override void Idle()
    {
        if (agent.destination != initialPosition)
            ResetPosition();
    }
}