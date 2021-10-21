using UnityEngine;
using UnityEngine.AI;

//Stop enemy infantry movement when player loses
public class AIInfantry : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private void OnEnable() => EventManager.OnGameEnded += StopEnemies;
    private void OnDisable() => EventManager.OnGameEnded -= StopEnemies;
    private void StopEnemies() => agent.SetDestination(transform.position);
}
