using System.Collections;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private PoolID enemyID;
    [SerializeField] private PoolID bossID;
    [SerializeField] private int startingEnemyCount;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private float raisePercentage;
    [SerializeField] private float waveDelay;
    [SerializeField] private int bossWaveFrequency;
    [SerializeField] private int difficultyModifierFrequency;
    private int currentWave;
    private float currentEnemyCount;
    private int activeEnemies;

    public void StartSpawning()
    {
        activeEnemies = 0;
        currentEnemyCount = startingEnemyCount;
        currentWave = 0;
        StartCoroutine(Spawn());
    }

    private void OnEnable() => EventManager.OnEnemyDisabled += EnemyKilled;
    private void OnDisable() => EventManager.OnEnemyDisabled -= EnemyKilled;
    private void EnemyKilled() => activeEnemies -= 1;

    private IEnumerator Spawn()
    {
        while (true)
        {
            currentWave += 1;

            if (currentWave % difficultyModifierFrequency == 0)
                EventManager.RaiseEnemyDifficulty();

            UIManager.Instance.ShowWaveNumber(currentWave);
            SpawnFormation();
            yield return new WaitUntil(() => activeEnemies == 0);
            if (currentWave % bossWaveFrequency == 0)
                yield return StartCoroutine(SpawnBoss());

            UIManager.Instance.ShowWaveCompleted();
            EventManager.RaiseWaveEnded();

            if (currentEnemyCount < maxEnemyCount)
                currentEnemyCount = Mathf.Clamp(currentEnemyCount * raisePercentage, startingEnemyCount, maxEnemyCount);

            yield return new WaitForSeconds(waveDelay);
        }
    }

    private IEnumerator SpawnBoss()
    {
        GameObject b = PoolManager.Instance.GetPooledObject(bossID, transform.position, transform.rotation);
        activeEnemies++;
        yield return new WaitUntil(() => !b.activeSelf);
    }

    private void SpawnFormation()
    {
        for (int i = 0; i < currentEnemyCount; i++)
        {
            PoolManager.Instance.GetPooledObject(enemyID, transform.position, transform.rotation);
            activeEnemies += 1;
        }
    }
}