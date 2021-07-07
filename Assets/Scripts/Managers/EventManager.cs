public static class EventManager
{
    public delegate void EnemyKilled(int coins);
    public static event EnemyKilled OnEnemyKilled;
    public static void RaiseEnemyKilled(int coins) => OnEnemyKilled?.Invoke(coins);

    public delegate void EnemyDisabled();
    public static event EnemyDisabled OnEnemyDisabled;
    public static void RaiseEnemyDisabled() => OnEnemyDisabled?.Invoke();
}