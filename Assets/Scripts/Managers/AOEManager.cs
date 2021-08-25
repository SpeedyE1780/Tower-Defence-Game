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
        NativeArray<AOEDamagedUnit> damaged = new NativeArray<AOEDamagedUnit>(unitsInfo.Count, Allocator.TempJob);

        for (int i = 0; i < units.Length; i++)
            damaged[i] = new AOEDamagedUnit() { UnitID = units[i].InstanceID };


        AOEJob aoe = new AOEJob()
        {
            units = units,
            affectedUnits = damaged,
            aoePosition = position,
            range = range,
            damage = damage
        };

        JobHandle aoeHandle = aoe.Schedule(unitsInfo.Count, 75);
        aoeHandle.Complete();


        foreach (AOEDamagedUnit unit in damaged)
            if (unit.Damage > 0)
                activeUnits[unit.UnitID].TakeHit(unit.Damage);

        units.Dispose();
        damaged.Dispose();
    }
}