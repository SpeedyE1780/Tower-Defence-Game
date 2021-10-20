using UnityEngine;
using UnityEngine.AI;

//Reset ally infantry position to starting position at the end of each wave
public class PlayerInfantry : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    private Vector3 initialPosition;

    private void OnEnable()
    {
        EventManager.OnWaveEnded += ResetPosition;
        initialPosition = transform.position;
    }

    private void OnDisable()
    {
        EventManager.OnWaveEnded -= ResetPosition;
    }

    private void ResetPosition() => agent.SetDestination(initialPosition);
}
