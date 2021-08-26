using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class UnitsManager : Singleton<UnitsManager>
{
    [SerializeField] ActiveUnitsSet activeUnits;
    [SerializeField] UnitsInfoSet unitsInfo;
    [SerializeField] UnitsInfoSet closestSet;
    [SerializeField] UnitsInfoSet mostInjuredSet;

    private void Start()
    {
        int maxUnitCount = UnitPlacementManager.MaximumUnits + SpawnManager.Instance.MaxEnemyCount;
        activeUnits.Initialize(maxUnitCount);
        unitsInfo.InitializeUnitsInfo(maxUnitCount);
        closestSet.InitializeUnitsInfo(maxUnitCount);
        mostInjuredSet.InitializeUnitsInfo(maxUnitCount);
    }

    private void Update()
    {
        NativeArray<UnitInfo> closestUnits = closestSet.GetJobArray();
        NativeArray<UnitInfo> mostInjuredUnits = mostInjuredSet.GetJobArray();
        NativeArray<UnitInfo> readOnly = unitsInfo.GetJobArray();

        FindClosestUnitJob closestUnitsJob = new FindClosestUnitJob()
        {
            unitInfo = closestUnits,
            othersInfo = readOnly
        };

        FindMostInjuredUnitJob mostInjuredUnitJob = new FindMostInjuredUnitJob()
        {
            unitInfo = mostInjuredUnits,
            othersInfo = readOnly
        };

        //Schedule and complete detection jobs
        JobHandle closestHandle = closestUnitsJob.Schedule(closestUnits.Length, 75);
        JobHandle mostInjuredHandle = mostInjuredUnitJob.Schedule(mostInjuredUnits.Length, 75);
        closestHandle.Complete();
        mostInjuredHandle.Complete();

        //Update units info
        unitsInfo.UpdateUnits(closestUnits);
        unitsInfo.UpdateUnits(mostInjuredUnits);

        //Dispose of the native lists
        closestUnits.Dispose();
        mostInjuredUnits.Dispose();
        readOnly.Dispose();
    }

    //Add unit to active units dictionary and native dictionary
    public void AddUnit(UnitInfo info, HealthController controller)
    {
        if (!unitsInfo.IsCreated)
            return;

        unitsInfo.Add(info);
        activeUnits.Add(info.InstanceID, controller);
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
    public void UpdateUnitPosition(int instanceID, Vector3 position, float healthPercentage)
    {
        unitsInfo.UpdateUnitInfo(instanceID, position, healthPercentage);
    }

    //Get target from native dictionary info
    public HealthController GetTarget(int instanceID)
    {
        //Get transform using target id
        int targetID = unitsInfo.GetTargetID(instanceID);
        return activeUnits[targetID];
    }

    private void OnDestroy()
    {
        unitsInfo.Dispose();
        closestSet.Dispose();
        mostInjuredSet.Dispose();
    }
}