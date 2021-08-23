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

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.OnWaveEnded -= ResetPosition;
    }

    //Move unit back to its starting position
    private void ResetPosition() => agent.SetDestination(initialPosition);

    protected override void Idle()
    {
        if (agent.destination != initialPosition)
            ResetPosition();
    }
}