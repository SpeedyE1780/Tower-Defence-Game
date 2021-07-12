public static class EventManager
{
    public delegate void EnemyKilled(int coins);
    public static event EnemyKilled OnEnemyKilled;
    public static void RaiseEnemyKilled(int coins) => OnEnemyKilled?.Invoke(coins);

    public delegate void EnemyDisabled();
    public static event EnemyDisabled OnEnemyDisabled;
    public static void RaiseEnemyDisabled() => OnEnemyDisabled?.Invoke();

    public delegate void WaveEnded();
    public static event WaveEnded OnWaveEnded;
    public static void RaiseWaveEnded() => OnWaveEnded?.Invoke();

    public delegate void RaiseDifficulty();
    public static event RaiseDifficulty OnRaiseDifficulty;
    public static void RaiseEnemyDifficulty() => OnRaiseDifficulty?.Invoke();
}