using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int meduimThreshold;
    [SerializeField] private int hardThreshold;

    private int kills = 0;
    private GameDifficulty difficulty;

    private void Start()
    {
        UIManager.Instance.UpdateKillText(kills);
        difficulty = GameDifficulty.Easy;
        SpawnManager.Instance.SetFormationsDifficulty(difficulty);
    }

    private void OnEnable() => EventManager.OnEnemyKilled += AddPoints;
    private void OnDisable() => EventManager.OnEnemyKilled -= AddPoints;
    private void AddPoints(int coins)
    {
        kills += 1;
        UIManager.Instance.UpdateKillText(kills);
        ShopManager.Instance.UpdateCurrency(coins);
        UpdateGameDifficulty();
    }

    private void UpdateGameDifficulty()
    {
        if (kills >= hardThreshold && difficulty == GameDifficulty.Meduim)
        {
            difficulty = GameDifficulty.Hard;
            SpawnManager.Instance.SetFormationsDifficulty(difficulty);
        }
        else if (kills >= meduimThreshold && difficulty == GameDifficulty.Easy)
        {
            difficulty = GameDifficulty.Meduim;
            SpawnManager.Instance.SetFormationsDifficulty(difficulty);
        }
    }
}

public enum GameDifficulty { Easy, Meduim, Hard }