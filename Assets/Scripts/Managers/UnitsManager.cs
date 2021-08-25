using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class UnitsManager : Singleton<UnitsManager>
{
    [SerializeField] SerializableDictionaryBase<UnitID, UnitsInfoSet> unitsSet;
    [SerializeField] ActiveUnitsSet activeUnits;
    [SerializeField] UnitsInfoSet unitsInfo;
    [SerializeField] UnitsInfoSet distanceSet;
    [SerializeField] UnitsInfoSet healerSet;

    private void Start()
    {
        int maxUnitCount = UnitPlacementManager.MaximumUnits + SpawnManager.Instance.MaxEnemyCount;
        activeUnits.Initialize(maxUnitCount);
        unitsInfo.InitializeUnitsInfo(maxUnitCount);
        distanceSet.InitializeUnitsInfo(maxUnitCount);
        healerSet.InitializeUnitsInfo(maxUnitCount);
    }

    private void Update()
    {
        NativeArray<UnitInfo> distanceUnits = distanceSet.GetJobArray();
        NativeArray<UnitInfo> healerUnits = healerSet.GetJobArray();
        NativeArray<UnitInfo> readOnly = unitsInfo.GetJobArray();

        DistanceDetectionJob distanceDetection = new DistanceDetectionJob()
        {
            unitInfo = distanceUnits,
            othersInfo = readOnly
        };

        HealerDetectionJob healerDetection = new HealerDetectionJob()
        {
            unitInfo = healerUnits,
            othersInfo = readOnly
        };

        //Schedule and complete detection jobs
        JobHandle detectionHandle = distanceDetection.Schedule(distanceUnits.Length, 75);
        JobHandle healerHandle = healerDetection.Schedule(healerUnits.Length, 75);
        detectionHandle.Complete();
        healerHandle.Complete();

        //Update units info
        unitsInfo.UpdateUnits(distanceUnits);
        unitsInfo.UpdateUnits(healerUnits);

        //Dispose of the native lists
        distanceUnits.Dispose();
        healerUnits.Dispose();
        readOnly.Dispose();
    }

    //Add unit to active units dictionary and native dictionary
    public void AddUnit(UnitInfo info, HealthController controller, UnitID id)
    {
        if (!unitsInfo.IsCreated)
            return;

        unitsInfo.Add(info);
        unitsSet[id].Add(info);
        activeUnits.Add(info.InstanceID, controller);
    }

    //Remove unit from active units and native dictionary
    public void RemoveUnit(int instanceID, UnitID id)
    {
        if (!unitsInfo.IsCreated)
            return;

        unitsInfo.Remove(instanceID);
        activeUnits.Remove(instanceID);
        unitsSet[id].Remove(instanceID);
    }

    //Update units position in native dictionary
    public void UpdateUnitPosition(int instanceID, Vector3 position, float healthPercentage, UnitID id)
    {
        unitsInfo.UpdateUnitInfo(instanceID, position, healthPercentage);
        unitsSet[id].UpdateUnitInfo(instanceID, position, healthPercentage);
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
        healerSet.Dispose();
    }
}