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

    public int MaxEnemyCount => maxEnemyCount;

    public void StartSpawning()
    {
        //Reset spawning variables
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
                currentEnemyCount = Mathf.Clamp(currentEnemyCount * raisePercentage, startingEnemyCount, maxEnemyCount);

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
        Rigidbody rb = SpawnEnemy(bossID, transform.position, transform.rotation);
        activeEnemies++;

        //Wait for boss or ally units to die
        yield return new WaitUntil(() => !rb.gameObject.activeSelf || UnitPlacementManager.UnitCount == 0);
    }

    private void SpawnFormation()
    {
        Transform child;
        for (int i = 0; i < currentEnemyCount; i++)
        {
            //Get position and rotation from children
            child = transform.GetChild(i);
            SpawnEnemy(enemyID, child.position, child.rotation);
            activeEnemies += 1;
        }
    }

    //Get enemy from pool and update its rigidbody position and rotation
    private Rigidbody SpawnEnemy(PoolID id, Vector3 position, Quaternion rotation)
    {
        Rigidbody rb = PoolManager.Instance.GetPooledObject<Rigidbody>(id, position, rotation);
        rb.MovePosition(position);
        rb.MoveRotation(rotation);
        rb.gameObject.SetActive(true);
        return rb;
    }

    private void ToggleUnitPlacement(bool toggle)
    {
        UIManager.Instance.ToggleUnitPlacementCanvas(toggle);
        PlacementManager.SetCanPlaceUnits(toggle);
    }

    private IEnumerator WaitForWaveDelay()
    {
        float time = waveDelay;

        //Wait for wave delay to end
        while (time > 0)
        {
            UIManager.Instance.SetWaveDelay((int)time + 1);
            yield return null;
            time -= Time.deltaTime;
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