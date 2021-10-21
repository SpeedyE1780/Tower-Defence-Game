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
        NativeArray<UnitInfo> closestUnits = closestSet.GetJobArray(); //Units that target closest units
        NativeArray<UnitInfo> mostInjuredUnits = mostInjuredSet.GetJobArray(); //Units that most injured units
        NativeArray<UnitInfo> readOnly = unitsInfo.GetJobArray(); //All active units

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
        activeUnits.Add(info.instanceID, controller);
    }

    //Remove unit from active units and native dictionary
    public void RemoveUnit(int instanceID)
    {
        if (!unitsInfo.IsCreated)
            return;

        unitsInfo.Remove(instanceID);
        activeUnits.Remove(instanceID);
    }

    //Update units info in native dictionary
    public void UpdateUnitInfo(int instanceID, Vector3 position, float healthPercentage) => unitsInfo.UpdateUnitInfo(instanceID, position, healthPercentage);

    //Get target from native dictionary info using target id
    public HealthController GetTarget(int instanceID)
    {
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
