using UnityEngine;

public class EnemyController : InfantryController
{
    private static Vector3 destination;
    private const string DestinationTag = "Respawn";

    [Header("Enemy Stats")]
    [SerializeField] private PoolID id;
    [SerializeField] private int coins;
    [SerializeField] private HealthController health;

    protected override bool HasIdleUpdate => true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void SetDestination() => destination = GameObject.FindGameObjectWithTag(DestinationTag).transform.position;

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.SetDestination(destination);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.RaiseEnemyDisabled();

        if (health.Health <= 0)
            EventManager.RaiseEnemyKilled(coins);
    }

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