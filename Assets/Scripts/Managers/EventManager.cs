public static class EventManager
{
    public delegate void EnemyKilled(int points, int coins);
    public static event EnemyKilled OnEnemyKilled;
    public static void RaiseEnemyKilled(int points, int coins) => OnEnemyKilled?.Invoke(points, coins);

    public delegate void EnemyDisabled();
    public static event EnemyDisabled OnEnemyDisabled;
    public static void RaiseEnemyDisabled() => OnEnemyDisabled?.Invoke();
}