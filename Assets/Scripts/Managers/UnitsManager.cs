using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnitData = Unity.Collections.NativeHashMap<int, UnitInfo>;

public class UnitsManager : Singleton<UnitsManager>
{
    private static Dictionary<int, Transform> activeUnits;
    private static UnitData troopInfo; //Contains all troops info
    private static UnitData enemyInfo; //Contains all enemy info

    private void Start()
    {
        activeUnits = new Dictionary<int, Transform>();
        troopInfo = new UnitData(UnitPlacementManager.MaximumUnits, Allocator.Persistent);
        enemyInfo = new UnitData(SpawnManager.Instance.MaxEnemyCount, Allocator.Persistent);
    }

    private void Update()
    {
        //Create job handle list and get enemies and troops info
        NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(troopInfo.Count() + enemyInfo.Count(), Allocator.Temp);
        NativeArray<UnitInfo> enemies = enemyInfo.GetValueArray(Allocator.TempJob);
        NativeArray<UnitInfo> troops = troopInfo.GetValueArray(Allocator.TempJob);

        DetectionJob troopDetection = new DetectionJob()
        {
            enemyInfo = enemies,
            unitInfo = troops
        };

        DetectionJob enemyDetection = new DetectionJob()
        {
            enemyInfo = troops,
            unitInfo = enemies
        };

        //Schedule detection jobs
        JobHandle troopHandle = troopDetection.Schedule(troops.Length, 50);
        JobHandle enemyHandle = enemyDetection.Schedule(enemies.Length, 100, troopHandle);

        //Add jobs to list and complete them all
        jobHandles.Add(troopHandle);
        jobHandles.Add(enemyHandle);
        JobHandle.CompleteAll(jobHandles);

        //Update enemies info
        foreach (UnitInfo unitInfo in enemies)
            enemyInfo[unitInfo.InstanceID] = unitInfo;

        //Update troops info
        foreach (UnitInfo unitInfo in troops)
            troopInfo[unitInfo.InstanceID] = unitInfo;

        //Dispose of the native lists
        jobHandles.Dispose();
        enemies.Dispose();
        troops.Dispose();
    }

    //Add unit to active units dictionary and native dictionary
    public static void AddUnit(bool isEnemy, UnitInfo info, Transform transform)
    {
        if (!enemyInfo.IsCreated || !troopInfo.IsCreated)
            return;

        if (isEnemy)
            enemyInfo.Add(info.InstanceID, info);
        else
            troopInfo.Add(info.InstanceID, info);

        activeUnits.Add(info.InstanceID, transform);
    }

    //Remove unit from active units and native dictionary
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

    //Update units position in native dictionary
    public static void UpdateUnitPosition(bool isEnemy, int instanceID, Vector3 position)
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

    //Get target from native dictionary info
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

        //Get transform using target id
        activeUnits.TryGetValue(targetID, out Transform transform);
        return transform;
    }

    private void OnDestroy()
    {
        enemyInfo.Dispose();
        troopInfo.Dispose();
    }
}