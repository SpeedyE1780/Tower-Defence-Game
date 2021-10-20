using Unity.Collections;
using UnityEngine;
using UnitData = Unity.Collections.NativeHashMap<int, UnitInfo>;

[CreateAssetMenu(menuName = "Units/Set/Info")]
public class UnitsInfoSet : ScriptableObject
{
    //Contains all units info in set
    private UnitData unitsInfo;

    public int Count { get; private set; }
    public bool IsCreated => unitsInfo.IsCreated;

    #region SETUP

    public void InitializeUnitsInfo(int capacity)
    {
        unitsInfo = new UnitData(capacity, Allocator.Persistent);
        Count = 0;
    }

    //Add unit once enabled
    public void Add(UnitInfo info)
    {
        if (!IsCreated)
            return;

        unitsInfo.Add(info.instanceID, info);
        Count += 1;
    }

    //Update unit info every frame
    public void UpdateUnitInfo(int ID, Vector3 position, float healthPercentage)
    {
        UnitInfo info = unitsInfo[ID];
        info.position = position;
        info.healthPercentage = healthPercentage;
        unitsInfo[ID] = info;
    }

    //Remove units once disabled
    public void Remove(int ID)
    {
        if (!IsCreated)
            return;

        unitsInfo.Remove(ID);
        Count -= 1;
    }

    #endregion

    #region JOB FUNCTIONS

    //Get value array to use in units manager jobs
    public NativeArray<UnitInfo> GetJobArray() => unitsInfo.GetValueArray(Allocator.TempJob);

    //Update unit info after jobs are completed in units manager
    public void UpdateUnits(NativeArray<UnitInfo> newInfo)
    {
        foreach (UnitInfo info in newInfo)
            unitsInfo[info.instanceID] = info;
    }

    #endregion

    #region UTILITY

    public int GetTargetID(int ID) => unitsInfo[ID].targetID;
    public void Dispose() => unitsInfo.Dispose();

    #endregion
}
