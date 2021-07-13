using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int kills;

    public void StartGame()
    {
        kills = 0;
        UIManager.Instance.UpdateKillText(kills);
        UIManager.Instance.ShowGameUI();
        SpawnManager.Instance.StartSpawning();
    }

    public void QuitGame() => Application.Quit();

    private void OnEnable() => EventManager.OnEnemyKilled += AddPoints;
    private void OnDisable() => EventManager.OnEnemyKilled -= AddPoints;
    private void AddPoints(int coins)
    {
        kills += 1;
        UIManager.Instance.UpdateKillText(kills);
        ShopManager.Instance.UpdateCurrency(coins);
    }
}