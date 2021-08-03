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
            yield return StartCoroutine(WaveDelay());

            currentWave += 1;

            if (currentWave % difficultyModifierFrequency == 0)
                EnemyManager.IncrementMultiplier();

            UIManager.Instance.ShowWaveNumber(currentWave);
            SpawnFormation();
            yield return new WaitUntil(() => activeEnemies == 0);
            if (currentWave % bossWaveFrequency == 0)
                yield return StartCoroutine(SpawnBoss());

            UIManager.Instance.ShowWaveCompleted();
            EventManager.RaiseWaveEnded();

            if (currentEnemyCount < maxEnemyCount)
                currentEnemyCount = Mathf.Clamp(currentEnemyCount * raisePercentage, startingEnemyCount, maxEnemyCount);
        }
    }

    private IEnumerator WaveDelay()
    {
        yield return UIManager.Instance.ShowPlaceUnits();

        UIManager.Instance.ToggleUnitPlacementCanvas(true);
        PlacementManager.SetCanPlaceUnits(true);
        float time = waveDelay;

        while (time > 0)
        {
            UIManager.Instance.SetWaveDelay((int)time);
            yield return null;
            time -= Time.deltaTime;
        }

        UIManager.Instance.ToggleUnitPlacementCanvas(false);
        PlacementManager.SetCanPlaceUnits(false);
    }

    private IEnumerator SpawnBoss()
    {
        Rigidbody rb = PoolManager.Instance.GetPooledObject<Rigidbody>(bossID, transform.position, transform.rotation);
        rb.MovePosition(transform.position);
        rb.MoveRotation(transform.rotation);
        rb.gameObject.SetActive(true);
        activeEnemies++;
        yield return new WaitUntil(() => !rb.gameObject.activeSelf);
    }

    private void SpawnFormation()
    {
        Transform child;
        for (int i = 0; i < currentEnemyCount; i++)
        {
            child = transform.GetChild(i);
            Rigidbody rb = PoolManager.Instance.GetPooledObject<Rigidbody>(enemyID, child.position, child.rotation);
            rb.MovePosition(child.position);
            rb.MoveRotation(child.rotation);
            rb.gameObject.SetActive(true);
            activeEnemies += 1;
        }
    }

    [ContextMenu("SpawnRandomPoint")]
    public void SpawnRandomPosition()
    {
        for (int i = 0; i < maxEnemyCount; i++)
        {
            GameObject gameObject = new GameObject($"Point {i}");
            gameObject.transform.position = new Vector3(Random.Range(-37.5f, 37.5f), 0, Random.Range(-25f, 25f));
        }
    }
}