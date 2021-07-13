public static class EnemyManager
{
    public static int Multiplier { get; private set; }
    public static void InitializeMultiplier() => Multiplier = 1;
    public static void IncrementMultiplier() => Multiplier += 1;
}