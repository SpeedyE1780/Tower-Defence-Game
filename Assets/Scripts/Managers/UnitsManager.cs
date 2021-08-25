using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class UnitsManager : Singleton<UnitsManager>
{
    private static Dictionary<int, Transform> activeUnits;
    [SerializeField] UnitsSet unitsInfo;

    private void Start()
    {
        activeUnits = new Dictionary<int, Transform>();
        unitsInfo.InitializeUnitsInfo(UnitPlacementManager.MaximumUnits + SpawnManager.Instance.MaxEnemyCount);
    }

    private void Update()
    {
        //Create job handle list and get units info
        NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(unitsInfo.Count, Allocator.Temp);
        NativeArray<UnitInfo> units = unitsInfo.GetJobArray();
        NativeArray<UnitInfo> readOnly = unitsInfo.GetJobArray();

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
        unitsInfo.UpdateUnits(units);

        //Dispose of the native lists
        jobHandles.Dispose();
        units.Dispose();
        readOnly.Dispose();
    }

    //Add unit to active units dictionary and native dictionary
    public void AddUnit(UnitInfo info, Transform transform)
    {
        if (!unitsInfo.IsCreated)
            return;

        unitsInfo.Add(info);
        activeUnits.Add(info.InstanceID, transform);
    }

    //Remove unit from active units and native dictionary
    public void RemoveUnit(int instanceID)
    {
        if (!unitsInfo.IsCreated)
            return;

        unitsInfo.Remove(instanceID);
        activeUnits.Remove(instanceID);
    }

    //Update units position in native dictionary
    public void UpdateUnitPosition(int instanceID, Vector3 position)
    {
        unitsInfo.UpdateUnitPosition(instanceID, position);
    }

    //Get target from native dictionary info
    public Transform GetTarget(int instanceID)
    {
        //Get transform using target id
        int targetID = unitsInfo.GetTargetID(instanceID);
        activeUnits.TryGetValue(targetID, out Transform transform);
        return transform;
    }

    private void OnDestroy()
    {
        unitsInfo.Dispose();
    }
}