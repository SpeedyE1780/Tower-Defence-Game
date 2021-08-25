using Unity.Collections;
using UnityEngine;
using UnitData = Unity.Collections.NativeHashMap<int, UnitInfo>;

[CreateAssetMenu(menuName = "Units/Set")]
public class UnitsSet : ScriptableObject
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
            unitsInfo[info.InstanceID] = info;
    }

    public void Add(UnitInfo info)
    {
        if (!IsCreated)
            return;

        unitsInfo.Add(info.InstanceID, info);
    }

    public void Remove(int ID)
    {
        if (!IsCreated)
            return;

        unitsInfo.Remove(ID);
    }

    public void UpdateUnitPosition(int ID, Vector3 position)
    {
        UnitInfo info = unitsInfo[ID];
        info.Position = position;
        unitsInfo[ID] = info;
    }

    public int GetTargetID(int ID)
    {
        return unitsInfo[ID].TargetID;
    }

    public void Dispose()
    {
        unitsInfo.Dispose();
    }
}