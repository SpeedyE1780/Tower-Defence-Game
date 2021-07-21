public static class EnemyManager
{
    private const float MultiplierRaise = 0.15f;
    public static float Multiplier { get; private set; }
    public static void InitializeMultiplier() => Multiplier = 1;
    public static void IncrementMultiplier() => Multiplier += MultiplierRaise;
}