using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class EnemyManager : Singleton<EnemyManager>
{
    List<Transform> activeEnemies;
    Transform[] activeEnemiesArray;
    private Dictionary<Transform, float> enemySpeed;
    private TransformAccessArray enemies;
    NativeArray<Vector3> direction;
    NativeArray<float> speed;
    EnemyMovement move;

    private void OnEnable()
    {
        activeEnemies = new List<Transform>();
        enemySpeed = new Dictionary<Transform, float>();
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
    }

    private void Update()
    {
        JobsMovement();
    }

    private void JobsMovement()
    {
        move.Schedule(enemies).Complete();
    }

    public void AddEnemy(Transform enemy)
    {
        activeEnemies.Add(enemy);
        activeEnemiesArray = activeEnemies.ToArray();
        enemySpeed.Add(enemy, enemy.GetComponent<EnemyController>().Speed);
        SetEnemies();
        SetNativeArrays();
        SetJobs();
    }

    public void RemoveEnemy(Transform enemy)
    {
        activeEnemies.Remove(enemy);
        enemySpeed.Remove(enemy);
        activeEnemiesArray = activeEnemies.ToArray();
        SetEnemies();
        SetNativeArrays();
        SetJobs();
    }

    private void SetEnemies()
    {
        if (enemies.isCreated)
            enemies.Dispose();

        enemies = new TransformAccessArray(activeEnemiesArray);
    }

    private void SetNativeArrays()
    {
        if (direction.IsCreated)
            direction.Dispose();
        if (speed.IsCreated)
            speed.Dispose();

        direction = new NativeArray<Vector3>(activeEnemies.Count, Allocator.Persistent);
        speed = new NativeArray<float>(activeEnemies.Count, Allocator.Persistent);
        Transform current;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            current = activeEnemies[i];
            direction[i] = current.forward;
            speed[i] = enemySpeed[current];
        }
    }

    private void SetJobs()
    {
        move = new EnemyMovement()
        {
            deltaTime = Time.deltaTime,
            speed = speed,
            direction = direction
        };
    }

    public void OnDestroy()
    {
        if (direction.IsCreated)
            direction.Dispose();

        if (speed.IsCreated)
            speed.Dispose();

        if (enemies.isCreated)
            enemies.Dispose();

        Instance = null;
    }
}

[BurstCompile]
public struct EnemyMovement : IJobParallelForTransform
{
    public float deltaTime;
    public NativeArray<Vector3> direction;
    public NativeArray<float> speed;

    public void Execute(int index, TransformAccess transform)
    {
        transform.position += direction[index] * (speed[index] * deltaTime);
    }
}