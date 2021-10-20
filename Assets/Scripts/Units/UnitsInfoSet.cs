using Unity.Collections;
using UnityEngine;
using UnitData = Unity.Collections.NativeHashMap<int, UnitInfo>;

[CreateAssetMenu(menuName = "Units/Set/Info")]
public class UnitsInfoSet : ScriptableObject
{
    //Contains all units info
    private UnitData unitsInfo;

    public int Count { get; private set; }
    public bool IsCreated => unitsInfo.IsCreated;

    public void InitializeUnitsInfo(int capacity)
    {
        unitsInfo = new UnitData(capacity, Allocator.Persistent);
        Count = 0;
    }

    public NativeArray<UnitInfo> GetJobArray()
    {
        return unitsInfo.GetValueArray(Allocator.TempJob);
    }

    public void UpdateUnits(NativeArray<UnitInfo> newInfo)
    {
        foreach (UnitInfo info in newInfo)
            unitsInfo[info.instanceID] = info;
    }

    public void Add(UnitInfo info)
    {
        if (!IsCreated)
            return;

        unitsInfo.Add(info.instanceID, info);
        Count += 1;
    }

    public void Remove(int ID)
    {
        if (!IsCreated)
            return;

        unitsInfo.Remove(ID);
        Count -= 1;
    }

    public void UpdateUnitInfo(int ID, Vector3 position, float healthPercentage)
    {
        UnitInfo info = unitsInfo[ID];
        info.position = position;
        info.healthPercentage = healthPercentage;
        unitsInfo[ID] = info;
    }

    public int GetTargetID(int ID)
    {
        return unitsInfo[ID].targetID;
    }

    public void Dispose()
    {
        unitsInfo.Dispose();
    }
}