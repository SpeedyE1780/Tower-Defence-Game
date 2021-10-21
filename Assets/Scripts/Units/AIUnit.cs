using UnityEngine;

public class AIUnit : MonoBehaviour
{
    [SerializeField] private UnitID id;
    [SerializeField] private int coins;
    [SerializeField] private HealthController health;

    private void OnEnable()
    {
        //Update max health based on current multiplier
        health.UpdateMaxHealth(EnemyManager.Multiplier);
        EventManager.OnGameRestarted += PoolUnit;
    }

    private void OnDisable()
    {
        EventManager.OnGameRestarted -= PoolUnit;
        AddStats();
    }

    private void AddStats()
    {
        //Update enemy count in spawn manager
        EventManager.RaiseEnemyDisabled();

        //Add coins
        if (health.IsDead)
            EventManager.RaiseEnemyKilled(coins);
    }

    private void PoolUnit() => PoolManager.Instance.AddToPool(id, gameObject);
}
