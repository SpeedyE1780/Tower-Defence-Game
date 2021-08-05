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

    public delegate void ResetUnitIndex(int index);
    public static event ResetUnitIndex OnResetUnitIndex;
    public static void RaiseUnitIndex(int index) => OnResetUnitIndex?.Invoke(index);

    public delegate void GameEnded();
    public static event GameEnded OnGameEnded;
    public static void RaiseGameEnded() => OnGameEnded?.Invoke();
}