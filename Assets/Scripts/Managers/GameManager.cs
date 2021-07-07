using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int meduimThreshold;
    [SerializeField] private int hardThreshold;

    private int score = 0;
    private GameDifficulty difficulty;

    private void Start()
    {
        UIManager.Instance.UpdateScoreText(score);
        difficulty = GameDifficulty.Easy;
        SpawnManager.Instance.SetFormationsDifficulty(difficulty);
    }

    private void OnEnable() => EventManager.OnEnemyKilled += AddPoints;
    private void OnDisable() => EventManager.OnEnemyKilled -= AddPoints;
    private void AddPoints(int points, int coins)
    {
        score += points;
        UIManager.Instance.UpdateScoreText(score);
        ShopManager.Instance.UpdateCurrency(coins);
        UpdateGameDifficulty();
    }

    private void UpdateGameDifficulty()
    {
        if (score >= hardThreshold && difficulty == GameDifficulty.Meduim)
        {
            difficulty = GameDifficulty.Hard;
            SpawnManager.Instance.SetFormationsDifficulty(difficulty);
        }
        else if (score >= meduimThreshold && difficulty == GameDifficulty.Easy)
        {
            difficulty = GameDifficulty.Meduim;
            SpawnManager.Instance.SetFormationsDifficulty(difficulty);
        }
    }
}

public enum GameDifficulty { Easy, Meduim, Hard }