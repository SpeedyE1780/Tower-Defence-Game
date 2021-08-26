using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class UnitsManager : Singleton<UnitsManager>
{
    [SerializeField] ActiveUnitsSet activeUnits;
    [SerializeField] UnitsInfoSet unitsInfo;
    [SerializeField] UnitsInfoSet distanceSet;
    [SerializeField] UnitsInfoSet lowestHealthSet;

    private void Start()
    {
        int maxUnitCount = UnitPlacementManager.MaximumUnits + SpawnManager.Instance.MaxEnemyCount;
        activeUnits.Initialize(maxUnitCount);
        unitsInfo.InitializeUnitsInfo(maxUnitCount);
        distanceSet.InitializeUnitsInfo(maxUnitCount);
        lowestHealthSet.InitializeUnitsInfo(maxUnitCount);
    }

    private void Update()
    {
        NativeArray<UnitInfo> distanceUnits = distanceSet.GetJobArray();
        NativeArray<UnitInfo> lowestHealthUnits = lowestHealthSet.GetJobArray();
        NativeArray<UnitInfo> readOnly = unitsInfo.GetJobArray();

        FindClosestUnitJob distanceDetection = new FindClosestUnitJob()
        {
            unitInfo = distanceUnits,
            othersInfo = readOnly
        };

        FindMostInjuredUnitJob lowestHealthDetection = new FindMostInjuredUnitJob()
        {
            unitInfo = lowestHealthUnits,
            othersInfo = readOnly
        };

        //Schedule and complete detection jobs
        JobHandle detectionHandle = distanceDetection.Schedule(distanceUnits.Length, 75);
        JobHandle lowestHealthHandle = lowestHealthDetection.Schedule(lowestHealthUnits.Length, 75);
        detectionHandle.Complete();
        lowestHealthHandle.Complete();

        //Update units info
        unitsInfo.UpdateUnits(distanceUnits);
        unitsInfo.UpdateUnits(lowestHealthUnits);

        //Dispose of the native lists
        distanceUnits.Dispose();
        lowestHealthUnits.Dispose();
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
        distanceSet.Dispose();
        lowestHealthSet.Dispose();
    }
}