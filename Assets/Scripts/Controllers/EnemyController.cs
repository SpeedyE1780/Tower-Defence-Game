using UnityEngine;

public class EnemyController : InfantryController
{
    private static Vector3 destination;
    private const string DestinationTag = "Respawn";

    [Header("Enemy Stats")]
    [SerializeField] private PoolID id;
    [SerializeField] private int points;
    [SerializeField] private int coins;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void SetDestination() => destination = GameObject.FindGameObjectWithTag(DestinationTag).transform.position;

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.SetDestination(destination);
    }

    private void OnDisable() => EventManager.RaiseEnemyDisabled();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DestinationTag))
            PoolManager.Instance.AddToPool(id, gameObject);
    }

    protected override void Idle()
    {
        if (agent.destination != destination)
            agent.SetDestination(destination);
    }
}