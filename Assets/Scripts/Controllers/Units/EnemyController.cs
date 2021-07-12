using UnityEngine;

public class EnemyController : InfantryController
{
    private static Vector3 destination;
    private const string DestinationTag = "Respawn";

    [Header("Enemy Stats")]
    [SerializeField] private int coins;
    [SerializeField] private HealthController health;

    protected override bool HasIdleUpdate => true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void SetDestination() => destination = GameObject.FindGameObjectWithTag(DestinationTag).transform.position;

    private void OnEnable()
    {
        agent.SetDestination(destination);
        agent.speed = speed * SpawnManager.Instance.DifficultyModifier;
        health.RaiseHealth();
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
}