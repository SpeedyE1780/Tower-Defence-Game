using UnityEngine;

public class EnemyController : InfantryController
{
    [Header("Enemy Stats")]
    [SerializeField] private int coins;
    [SerializeField] private HealthController health;

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.speed = speed * EnemyManager.Multiplier;
        health.UpdateMaxHealth(EnemyManager.Multiplier);
        EventManager.OnGameEnded += StopEnemies;
    }

    private void OnDisable()
    {
        EventManager.OnGameEnded -= StopEnemies;
    }

    private void StopEnemies()
    {
        agent.SetDestination(transform.position);
    }

    public override void PoolUnit()
    {
        base.PoolUnit();
        EventManager.RaiseEnemyDisabled();

        if (health.IsDead)
            EventManager.RaiseEnemyKilled(coins);
    }
}