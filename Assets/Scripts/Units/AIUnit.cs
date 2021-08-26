using UnityEngine;

public class AIUnit : MonoBehaviour
{
    [SerializeField] private UnitID id;
    [SerializeField] private int coins;
    [SerializeField] private HealthController health;

    private void OnEnable()
    {
        health.RaiseMaxHealth(EnemyManager.Multiplier);
        EventManager.OnGameRestarted += PoolUnit;
    }

    private void OnDisable()
    {
        EventManager.OnGameRestarted -= PoolUnit;
        AddStats();
    }

    private void AddStats()
    {
        EventManager.RaiseEnemyDisabled();

        if (health.IsDead)
            EventManager.RaiseEnemyKilled(coins);
    }

    private void PoolUnit()
    {
        PoolManager.Instance.AddToPool(id, gameObject);
    }
}