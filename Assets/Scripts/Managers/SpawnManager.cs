using System.Collections;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private PoolID enemyID;
    [SerializeField] private PoolID bossID;
    [SerializeField] private Transform easyFormations;
    [SerializeField] private Transform meduimFormations;
    [SerializeField] private Transform hardFormations;
    [SerializeField] private float waveDelay;
    [SerializeField] private int bossWaveFrequency;
    private int activeEnemies;
    private int currentWave;
    private Transform currentFormations;

    private Transform GetRandomFormation => currentFormations.GetChild(Random.Range(0, currentFormations.childCount));

    void Start()
    {
        activeEnemies = 0;
        currentWave = 0;
        StartCoroutine(Spawn());
    }

    private void OnEnable()
    {
        EventManager.OnEnemyDisabled += EnemyKilled;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyDisabled -= EnemyKilled;
    }

    public void SetFormationsDifficulty(GameDifficulty difficulty)
    {
        currentFormations = difficulty switch
        {
            GameDifficulty.Easy => easyFormations,
            GameDifficulty.Meduim => meduimFormations,
            GameDifficulty.Hard => hardFormations,
            _ => throw new System.Exception("Difficulty Error"),
        };
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            currentWave += 1;
            StartCoroutine(UIManager.Instance.ShowWaveNumber(currentWave));
            SpawnFormation();
            yield return new WaitUntil(() => activeEnemies == 0);
            if (currentWave % bossWaveFrequency == 0)
                yield return StartCoroutine(SpawnBoss());

            StartCoroutine(UIManager.Instance.ShowWaveCompleted());
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private IEnumerator SpawnBoss()
    {
        GameObject b = PoolManager.Instance.GetPooledObject(bossID);
        b.transform.forward = Vector3.back;
        b.GetComponent<EnemyController>().SetEnemyController();
        activeEnemies++;
        yield return new WaitUntil(() => !b.activeSelf);
    }

    private void SpawnFormation()
    {
        Transform formation = GetRandomFormation;

        foreach (Transform subFormation in formation)
        {
            foreach (Transform point in subFormation)
            {
                GameObject enemy = PoolManager.Instance.GetPooledObject(enemyID);
                activeEnemies += 1;
                enemy.transform.SetPositionAndRotation(point.position, point.rotation);
                enemy.GetComponent<EnemyController>().SetEnemyController();
            }
        }
    }

    private void EnemyKilled()
    {
        activeEnemies -= 1;
    }
}