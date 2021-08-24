using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnitData = Unity.Collections.NativeHashMap<int, UnitInfo>;

public class UnitsManager : Singleton<UnitsManager>
{
    private static Dictionary<int, Transform> activeUnits;
    private static UnitData unitsInfo; //Contains all troops info

    private void Start()
    {
        activeUnits = new Dictionary<int, Transform>();
        unitsInfo = new UnitData(UnitPlacementManager.MaximumUnits, Allocator.Persistent);
    }

    private void Update()
    {
        //Create job handle list and get enemies and troops info
        NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(unitsInfo.Count(), Allocator.Temp);
        NativeArray<UnitInfo> units = unitsInfo.GetValueArray(Allocator.TempJob);
        NativeArray<UnitInfo> readOnly = unitsInfo.GetValueArray(Allocator.TempJob);

        DetectionJob troopDetection = new DetectionJob()
        {
            enemyInfo = readOnly,
            unitInfo = units
        };

        //Schedule detection jobs
        JobHandle troopHandle = troopDetection.Schedule(units.Length, 75);

        //Add jobs to list and complete them all
        jobHandles.Add(troopHandle);
        JobHandle.CompleteAll(jobHandles);

        //Update units info
        foreach (UnitInfo unitInfo in units)
            unitsInfo[unitInfo.InstanceID] = unitInfo;

        //Dispose of the native lists
        jobHandles.Dispose();
        units.Dispose();
        readOnly.Dispose();
    }

    //Add unit to active units dictionary and native dictionary
    public static void AddUnit(UnitInfo info, Transform transform)
    {
        if (!unitsInfo.IsCreated)
            return;

        unitsInfo.Add(info.InstanceID, info);
        activeUnits.Add(info.InstanceID, transform);
    }

    //Remove unit from active units and native dictionary
    public static void RemoveUnit(int instanceID)
    {
        if (!unitsInfo.IsCreated)
            return;

        unitsInfo.Remove(instanceID);
        activeUnits.Remove(instanceID);
    }

    //Update units position in native dictionary
    public static void UpdateUnitPosition(int instanceID, Vector3 position)
    {
        UnitInfo info = unitsInfo[instanceID];
        info.Position = position;
        unitsInfo[instanceID] = info;
    }

    //Get target from native dictionary info
    public static Transform GetTarget(int instanceID)
    {
        //Get transform using target id
        int targetID = unitsInfo[instanceID].TargetID;
        activeUnits.TryGetValue(targetID, out Transform transform);
        return transform;
    }

    private void OnDestroy()
    {
        unitsInfo.Dispose();
    }
}