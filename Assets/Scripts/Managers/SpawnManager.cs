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
    private WaitForSeconds waveDelayWait;
    private WaitUntil bossWaveWait;
    private GameObject currentBoss;

    private float ZOffset => transform.position.z;
    public int MaxEnemyCount => maxEnemyCount;

    #region UNITY MESSAGES

    private void Start()
    {
        waveDelayWait = new WaitForSeconds(1f);
        bossWaveWait = new WaitUntil(() => !currentBoss.activeSelf || UnitPlacementManager.UnitCount == 0);
    }

    private void OnEnable() => EventManager.OnEnemyDisabled += EnemyKilled;
    private void OnDisable() => EventManager.OnEnemyDisabled -= EnemyKilled;

    #endregion

    #region SPAWNING

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

            //Spawn formation and wait for player to position his units
            SpawnFormation();
            yield return StartCoroutine(WaveDelay());
            UnitController.waitForWaveStart = false;

            //Wait for all enemies to die
            while (activeEnemies > 0)
            {
                if (IsGameOver())
                    yield break;

                yield return null;
            }

            //Check if we should spawn a boss or no
            if (currentWave % bossWaveFrequency == 0)
            {
                //Spawn boss and wait for boss or ally units to die
                SpawnBoss();
                yield return bossWaveWait;

                if (IsGameOver())
                    yield break;
            }

            UnitController.waitForWaveStart = true;
            EventManager.RaiseWaveEnded();

            //Raise enemy count until we reach the max count
            if (currentEnemyCount < maxEnemyCount)
                currentEnemyCount = (int)Mathf.Clamp(currentEnemyCount * raisePercentage, startingEnemyCount, maxEnemyCount);

            //Wait for text popup
            yield return UIManager.Instance.ShowWaveCompleted();
        }
    }

    private void SpawnBoss()
    {
        //Update current boss and reset yield instruction
        currentBoss = SpawnEnemy(bossesID.GetRandomElement(), transform.position, transform.rotation);
        activeEnemies++;
        bossWaveWait.Reset();

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

    //Get enemy from pool and update its position and rotation
    private GameObject SpawnEnemy(PoolID id, Vector3 position, Quaternion rotation)
    {
        GameObject enemy = PoolManager.Instance.GetPooledObject(id, position, rotation);
        enemy.SetActive(true);
        return enemy;
    }

    private Vector3 GetSpawnPosition(int position)
    {
        int xIndex = position % unitsInRow;
        int zIndex = position / unitsInRow;
        Vector3 spawnPosition = new Vector3((xIndex - unitsInRow * 0.5f) * distanceBetweenUnits, 0, zIndex * distanceBetweenUnits + ZOffset);
        return spawnPosition;
    }

#if UNITY_EDITOR

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

#endif
    #endregion

    #region UTILITY

    public void SkipWaveDelay() => skipWait = true;
    private void EnemyKilled() => activeEnemies -= 1;

    private IEnumerator WaveDelay()
    {
        //Wait unitl pop up text disappears
        yield return UIManager.Instance.ShowWaveDelayUI(currentWave);
        ToggleUnitPlacement(true);
        yield return WaitForWaveDelay();
        ToggleUnitPlacement(false);

        //Change to default camera
        CameraManager.Instance.SetDefaultCamera();
    }

    private IEnumerator WaitForWaveDelay()
    {
        float time = waveDelay;
        skipWait = false;

        //Wait for wave delay to end or player pressing start wave button
        while (time > 0 && !skipWait)
        {
            UIManager.Instance.UpdateWaveDelay((int)time);
            yield return waveDelayWait;
            time -= 1;
        }
    }

    private void ToggleUnitPlacement(bool toggle)
    {
        UIManager.Instance.ToggleWaveDelayText(toggle);
        UIManager.Instance.ToggleUnitPlacementCanvas(toggle);
        PlacementManager.SetCanPlaceUnits(toggle);
    }

    private bool IsGameOver()
    {
        //Player lost all his units
        bool gameOver = UnitPlacementManager.UnitCount == 0;

        if (gameOver)
        {
            UIManager.Instance.ShowEndGameUI();
            EventManager.RaiseGameEnded();
        }

        return gameOver;
    }

    #endregion
}
