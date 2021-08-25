using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class UnitsManager : Singleton<UnitsManager>
{
    [SerializeField] ActiveUnitsSet activeUnits;
    [SerializeField] UnitsInfoSet unitsInfo;

    private void Start()
    {
        int maxUnitCount = UnitPlacementManager.MaximumUnits + SpawnManager.Instance.MaxEnemyCount;
        activeUnits.Initialize(maxUnitCount);
        unitsInfo.InitializeUnitsInfo(maxUnitCount);
    }

    private void Update()
    {
        NativeArray<UnitInfo> units = unitsInfo.GetJobArray();
        NativeArray<UnitInfo> readOnly = unitsInfo.GetJobArray();

        DetectionJob troopDetection = new DetectionJob()
        {
            enemyInfo = readOnly,
            unitInfo = units
        };

        //Schedule and complete detection jobs
        JobHandle detectionHandle = troopDetection.Schedule(units.Length, 75);
        detectionHandle.Complete();

        //Update units info
        unitsInfo.UpdateUnits(units);

        //Dispose of the native lists
        units.Dispose();
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
    public void UpdateUnitPosition(int instanceID, Vector3 position)
    {
        unitsInfo.UpdateUnitPosition(instanceID, position);
    }

    //Get target from native dictionary info
    public HealthController GetTarget(int instanceID)
    {
        //Get transform using target id
        int targetID = unitsInfo.GetTargetID(instanceID);
        return activeUnits.TryGetValue(targetID);
    }

    private void OnDestroy()
    {
        unitsInfo.Dispose();
    }
}