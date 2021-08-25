using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class AOEManager : Singleton<AOEManager>
{
    [SerializeField] ActiveUnitsSet activeUnits;
    [SerializeField] UnitsInfoSet unitsInfo;

    public void ApplyAOEDamage(Vector3 position, float range, int damage)
    {
        NativeArray<UnitInfo> units = unitsInfo.GetJobArray();
        NativeList<AOEDamagedUnit> damaged = new NativeList<AOEDamagedUnit>(unitsInfo.Count, Allocator.TempJob);

        AOEJob aoe = new AOEJob()
        {
            units = units,
            affectedUnits = damaged,
            aoePosition = position,
            range = range
        };

        JobHandle aoeHandle = aoe.Schedule(unitsInfo.Count, 75);
        aoeHandle.Complete();


        foreach (AOEDamagedUnit unit in damaged)
            activeUnits[unit.UnitID].TakeHit(unit.Damage);


        units.Dispose();
        damaged.Dispose();
    }
}