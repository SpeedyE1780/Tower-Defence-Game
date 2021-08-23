using UnityEngine;

public class EnemyController : InfantryController
{
    [Header("Enemy Stats")]
    [SerializeField] private int coins;
    [SerializeField] private HealthController health;

    protected override void OnEnable()
    {
        base.OnEnable();

        //Multiply agent speed and max health
        agent.speed = speed * EnemyManager.Multiplier;
        health.RaiseMaxHealth(EnemyManager.Multiplier);

        EventManager.OnGameRestarted += PoolUnit;
        EventManager.OnGameEnded += StopEnemies;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.OnGameEnded -= StopEnemies;
        EventManager.OnGameRestarted -= PoolUnit;
    }

    private void StopEnemies() => agent.SetDestination(transform.position);

    public override void PoolUnit()
    {
        base.PoolUnit();
        EventManager.RaiseEnemyDisabled();

        if (health.IsDead)
            EventManager.RaiseEnemyKilled(coins);
    }
}