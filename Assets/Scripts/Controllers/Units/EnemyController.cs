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
    private static void SetDestination()
    {
        GameObject g = GameObject.FindGameObjectWithTag(DestinationTag);
        if (g == null)
            return;

        destination = g.transform.position;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.speed = speed * EnemyManager.Multiplier;
        health.SetHealth(EnemyManager.Multiplier);
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
}