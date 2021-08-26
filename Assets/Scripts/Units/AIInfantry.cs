using UnityEngine;
using UnityEngine.AI;

public class AIInfantry : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private void OnEnable() => EventManager.OnGameEnded += StopEnemies;
    private void OnDisable() => EventManager.OnGameEnded -= StopEnemies;
    private void StopEnemies() => agent.SetDestination(transform.position);
}