using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int kills;

    private void OnEnable() => EventManager.OnEnemyKilled += AddPoints;
    private void OnDisable() => EventManager.OnEnemyKilled -= AddPoints;

    public void StartGame()
    {
        kills = 0;

        //Initialize Currency
        ShopManager.Instance.SetCurrency();

        //Reset unit count and reset enemy multiplier
        UnitPlacementManager.ResetUnitCount();
        EnemyManager.InitializeMultiplier();

        //Reset Game UI
        UIManager.Instance.UpdateKillText(kills);
        UIManager.Instance.ShowGameUI();

        SpawnManager.Instance.StartSpawning();
    }

    public void RestartGame()
    {
        EventManager.RaiseGameRestarted();
        StartGame();
    }

    //Increment kill count and add coins
    private void AddPoints(int coins)
    {
        kills += 1;
        UIManager.Instance.UpdateKillText(kills);
        ShopManager.Instance.UpdateCurrency(coins);
    }

    public void QuitGame() => Application.Quit();
}