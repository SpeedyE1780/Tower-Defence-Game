using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float physicsTimeStep;
    private bool simulatePhysics;
    private float physicsTimer;
    private int kills;

    public void StartGame()
    {
        ResetPhysics();
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

    public void QuitGame() => Application.Quit();
    private void OnEnable() => EventManager.OnEnemyKilled += AddPoints;
    private void OnDisable() => EventManager.OnEnemyKilled -= AddPoints;

    //Increment kill count and add coins
    private void AddPoints(int coins)
    {
        kills += 1;
        UIManager.Instance.UpdateKillText(kills);
        ShopManager.Instance.UpdateCurrency(coins);
    }

    private void ResetPhysics()
    {
        simulatePhysics = true;
        physicsTimer = 0;
        StartCoroutine(UpdatePhysics());
    }

    private IEnumerator UpdatePhysics()
    {
        while (simulatePhysics)
        {
            physicsTimer += Time.deltaTime;

            if (physicsTimer > physicsTimeStep)
            {
                Physics.Simulate(physicsTimer);
                physicsTimer = 0;
            }

            yield return null;
        }
    }
}