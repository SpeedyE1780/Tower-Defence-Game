using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private List<PoolID> groundedEnemiesID;
    [SerializeField] private List<PoolID> airborneEnemiesID;
    [SerializeField] private List<PoolID> bossesID;
    [SerializeField] private int startingEnemyCount;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private int unitsInRow;
    [SerializeField] private float distanceBetweenUnits;
    [SerializeField] private float raisePercentage;
    [SerializeField] private int waveDelay;
    [SerializeField] private int bossWaveFrequency;
    [SerializeField] private int difficultyModifierFrequency;
    private int currentWave;
    private bool skipWait;
    private int currentEnemyCount;
    private int activeEnemies;

    private float ZOffset => transform.position.z;
    public int MaxEnemyCount => maxEnemyCount;

    private void OnEnable() => EventManager.OnEnemyDisabled += EnemyKilled;
    private void OnDisable() => EventManager.OnEnemyDisabled -= EnemyKilled;
    public void SkipWaveDelay() => skipWait = true;
    private void EnemyKilled() => activeEnemies -= 1;

    public void StartSpawning()
    {
        //Reset spawning variables
        activeEnemies = 0;
        currentEnemyCount = startingEnemyCount;
        currentWave = 0;
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        UnitController.waitForWaveStart = true;
        UIManager.Instance.ToggleUnitPlacementCanvas(false);

        while (true)
        {
            currentWave += 1;

            //Check if we should raise the enemy health
            if (currentWave % difficultyModifierFrequency == 0)
                EnemyManager.IncrementMultiplier();

            SpawnFormation();
            yield return StartCoroutine(WaveDelay());
            UnitController.waitForWaveStart = false;

            //Wait for all enemies to die
            while (activeEnemies > 0)
            {
                //Player lost all his units
                if (UnitPlacementManager.UnitCount == 0)
                {
                    UIManager.Instance.ShowEndGameUI();
                    EventManager.RaiseGameEnded();
                    yield break;
                }

                yield return null;
            }

            //Check if we should spawn a boss or no
            if (currentWave % bossWaveFrequency == 0)
            {
                yield return StartCoroutine(SpawnBoss());

                //Player lost all his units
                if (UnitPlacementManager.UnitCount == 0)
                {
                    UIManager.Instance.ShowEndGameUI();
                    EventManager.RaiseGameEnded();
                    yield break;
                }
            }

            UnitController.waitForWaveStart = true;
            EventManager.RaiseWaveEnded();

            //Raise enemy count until we reach the max count
            if (currentEnemyCount < maxEnemyCount)
                currentEnemyCount = (int)Mathf.Clamp(currentEnemyCount * raisePercentage, startingEnemyCount, maxEnemyCount);

            yield return UIManager.Instance.ShowWaveCompleted();
        }
    }

    private IEnumerator WaveDelay()
    {
        //Wait unitl pop up text disappears
        UIManager.Instance.ShowWaveNumber(currentWave);
        yield return UIManager.Instance.ShowPlaceUnits();
        ToggleUnitPlacement(true);
        yield return WaitForWaveDelay();
        ToggleUnitPlacement(false);

        //Change to default camera
        CameraManager.Instance.SetDefaultCamera();
    }

    private IEnumerator SpawnBoss()
    {
        GameObject boss = SpawnEnemy(bossesID.GetRandomElement(), transform.position, transform.rotation);
        activeEnemies++;

        //Wait for boss or ally units to die
        yield return new WaitUntil(() => !boss.activeSelf || UnitPlacementManager.UnitCount == 0);
    }

    private void SpawnFormation()
    {
        Quaternion rotation = transform.rotation;
        int groundedEnemyCount = (int)(currentEnemyCount * Random.Range(0.35f, 0.5f));

        for (int i = 0; i < currentEnemyCount; i++)
        {
            PoolID enemyID = i < groundedEnemyCount ? groundedEnemiesID.GetRandomElement() : airborneEnemiesID.GetRandomElement();
            SpawnEnemy(enemyID, GetSpawnPosition(i), rotation);
            activeEnemies += 1;
        }
    }

    //Get enemy from pool and update its rigidbody position and rotation
    private GameObject SpawnEnemy(PoolID id, Vector3 position, Quaternion rotation)
    {
        GameObject enemy = PoolManager.Instance.GetPooledObject(id, position, rotation);
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    private void ToggleUnitPlacement(bool toggle)
    {
        UIManager.Instance.ToggleUnitPlacementCanvas(toggle);
        PlacementManager.SetCanPlaceUnits(toggle);
    }

    private IEnumerator WaitForWaveDelay()
    {
        float time = waveDelay;
        skipWait = false;

        //Wait for wave delay to end or player pressing start wave button
        while (time > 0 && !skipWait)
        {
            UIManager.Instance.SetWaveDelay((int)time + 1);
            yield return null;
            time -= Time.deltaTime;
        }
    }

    private Vector3 GetSpawnPosition(int position)
    {
        int xIndex = position % unitsInRow;
        int zIndex = position / unitsInRow;
        Vector3 spawnPosition = new Vector3((xIndex - unitsInRow * 0.5f) * distanceBetweenUnits, 0, zIndex * distanceBetweenUnits + ZOffset);
        return spawnPosition;
    }

    [ContextMenu("SpawnRandomPoint")]
    public void SpawnRandomPosition()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        for (int i = 0; i < maxEnemyCount; i++)
        {
            GameObject gameObject = new GameObject($"Point {i}");
            gameObject.transform.SetParent(transform);
            gameObject.transform.localPosition = GetSpawnPosition(i);
            gameObject.transform.localRotation = Quaternion.identity;
        }
    }
}