using System.Collections;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform easyFormations;
    [SerializeField] private Transform meduimFormations;
    [SerializeField] private Transform hardFormations;
    [SerializeField] private PoolManager enemyPool;
    [SerializeField] private float waveDelay;
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
        EventManager.OnEnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyKilled -= EnemyKilled;
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
            StartCoroutine(UIManager.Instance.ShowWaveCompleted());
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private void SpawnFormation()
    {
        Transform formation = GetRandomFormation;

        foreach (Transform subFormation in formation)
        {
            foreach (Transform point in subFormation)
            {
                GameObject enemy = enemyPool.Spawn();
                activeEnemies += 1;
                enemy.transform.SetPositionAndRotation(point.position, point.rotation);
                enemy.GetComponent<EnemyController>().SetEnemyController();
            }
        }
    }

    private void EnemyKilled(int points, int coins)
    {
        activeEnemies -= 1;
    }
}