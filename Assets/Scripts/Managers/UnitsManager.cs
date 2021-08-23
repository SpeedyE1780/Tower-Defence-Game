using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnitData = Unity.Collections.NativeHashMap<int, UnitInfo>;

public class UnitsManager : Singleton<UnitsManager>
{
    private static Dictionary<int, Transform> activeUnits;
    private static UnitData troopInfo;
    private static UnitData enemyInfo;

    private void Start()
    {
        activeUnits = new Dictionary<int, Transform>();
        troopInfo = new UnitData(UnitPlacementManager.MaximumUnits, Allocator.Persistent);
        enemyInfo = new UnitData(SpawnManager.Instance.MaxEnemyCount, Allocator.Persistent);
    }

    private void Update()
    {
        NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(troopInfo.Count() + enemyInfo.Count(), Allocator.Temp);
        NativeArray<UnitInfo> enemies = enemyInfo.GetValueArray(Allocator.TempJob);
        NativeArray<UnitInfo> troops = troopInfo.GetValueArray(Allocator.TempJob);

        DetectionJob troopDetection = new DetectionJob()
        {
            enemyInfo = enemies,
            unitInfo = troops,
        };

        DetectionJob enemyDetection = new DetectionJob()
        {
            enemyInfo = troops,
            unitInfo = enemies,
        };

        JobHandle troopHandle = troopDetection.Schedule(troops.Length, 50);
        JobHandle enemyHandle = enemyDetection.Schedule(enemies.Length, 100, troopHandle);
        jobHandles.Add(troopHandle);
        jobHandles.Add(enemyHandle);

        JobHandle.CompleteAll(jobHandles);

        foreach (UnitInfo unitInfo in enemies)
            enemyInfo[unitInfo.InstanceID] = unitInfo;

        foreach (UnitInfo unitInfo in troops)
            troopInfo[unitInfo.InstanceID] = unitInfo;

        jobHandles.Dispose();
        enemies.Dispose();
        troops.Dispose();
    }

    public static void AddUnit(bool isEnemy, UnitInfo info, Transform transform)
    {
        if (isEnemy)
            enemyInfo.Add(info.InstanceID, info);
        else
            troopInfo.Add(info.InstanceID, info);

        activeUnits.Add(info.InstanceID, transform);
    }

    public static void RemoveUnit(bool isEnemy, int instanceID)
    {
        if (!enemyInfo.IsCreated || !troopInfo.IsCreated)
            return;

        if (isEnemy)
            enemyInfo.Remove(instanceID);
        else
            troopInfo.Remove(instanceID);

        activeUnits.Remove(instanceID);
    }

    public static void UpdatePosition(bool isEnemy, int instanceID, Vector3 position)
    {
        if (isEnemy)
        {
            UnitInfo info = enemyInfo[instanceID];
            info.Position = position;
            enemyInfo[instanceID] = info;
        }
        else
        {
            UnitInfo info = troopInfo[instanceID];
            info.Position = position;
            troopInfo[instanceID] = info;
        }
    }

    public static Transform GetTarget(bool isEnemy, int instanceID)
    {
        int targetID;

        if (isEnemy)
        {
            targetID = enemyInfo[instanceID].TargetID;
        }
        else
        {
            targetID = troopInfo[instanceID].TargetID;
        }

        activeUnits.TryGetValue(targetID, out Transform transform);
        return transform;
    }

    private void OnDestroy()
    {
        enemyInfo.Dispose();
        troopInfo.Dispose();
    }
}

public struct UnitInfo
{
    public int InstanceID;
    public float3 Position;
    public int TargetID;
}

[BurstCompile]
public struct DetectionJob : IJobParallelFor
{
    public NativeArray<UnitInfo> unitInfo;
    [ReadOnly] public NativeArray<UnitInfo> enemyInfo;

    public void Execute(int index)
    {
        UnitInfo currentUnit = unitInfo[index];
        float3 position = currentUnit.Position;
        float currentDistance = math.INFINITY;

        foreach (UnitInfo enemy in enemyInfo)
        {
            float3 enemyPosition = enemy.Position;
            float temp = math.distance(position, enemyPosition);

            if (temp < currentDistance)
            {
                currentUnit.TargetID = enemy.InstanceID;
                currentDistance = temp;
            }
        }

        unitInfo[index] = currentUnit;
    }
}