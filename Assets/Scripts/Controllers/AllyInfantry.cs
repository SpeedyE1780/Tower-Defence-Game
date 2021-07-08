using System.Collections;
using UnityEngine;

public class AllyInfantry : InfantryController
{
    protected Vector3 initialPosition;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.OnWaveEnded += ResetPosition;
        StartCoroutine(GetInitialPosition());
    }

    private void OnDisable()
    {
        EventManager.OnWaveEnded -= ResetPosition;
    }

    private void ResetPosition() => agent.SetDestination(initialPosition);

    IEnumerator GetInitialPosition()
    {
        yield return new WaitForFixedUpdate();
        initialPosition = transform.position;
    }
}