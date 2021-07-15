using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float physicsTimeStep;
    private bool simulatePhysics;
    private float currentTimer;
    private int kills;

    public void StartGame()
    {
        simulatePhysics = true;
        kills = 0;
        currentTimer = 0;
        EnemyManager.InitializeMultiplier();
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

    private void Update()
    {
        if (simulatePhysics)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer > physicsTimeStep)
            {
                Physics.Simulate(currentTimer);
                currentTimer = 0;
            }
        }
    }
}