using UnityEngine;

public class EnemyController : InfantryController
{
    private static Vector3 destination;
    private const string DestinationTag = "Respawn";

    [Header("Enemy Stats")]
    [SerializeField] private int coins;
    [SerializeField] private HealthController health;

    private int multiplier;

    protected override bool HasIdleUpdate => true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void SetDestination() => destination = GameObject.FindGameObjectWithTag(DestinationTag).transform.position;

    protected override void Awake()
    {
        base.Awake();
        EventManager.OnRaiseDifficulty += RaiseStats;
        multiplier = 1;
    }

    private void RaiseStats()
    {
        multiplier += 1;
    }

    private void OnEnable()
    {
        agent.speed = speed * multiplier;
        health.SetHealth(multiplier);
        agent.SetDestination(destination);
    }

    public override void PoolUnit()
    {
        base.PoolUnit();
        EventManager.RaiseEnemyDisabled();

        if (health.IsDead)
            EventManager.RaiseEnemyKilled(coins);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DestinationTag))
            PoolUnit();
    }

    protected override void Idle()
    {
        if (agent.destination != destination)
            agent.SetDestination(destination);
    }

    private void OnDestroy()
    {
        EventManager.OnRaiseDifficulty -= RaiseStats;
    }
}