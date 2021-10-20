using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class AOEManager : Singleton<AOEManager>
{
    [SerializeField] ActiveUnitsSet activeUnits;
    [SerializeField] UnitsInfoSet unitsInfo;
    NativeArray<UnitInfo> units;
    NativeArray<AOEAffectedUnit> affectedUnits;
    NativeList<JobHandle> aoeJobHandles;

    public void ApplyAOEDamage(Vector3 position, float range, int damage, int unitMask)
    {
        //Create arrays if they're not created
        if (!units.IsCreated)
            InitializeArrays();

        AOEJob aoe = new AOEJob()
        {
            units = units,
            affectedUnits = affectedUnits,
            aoePosition = position,
            range = range,
            damage = damage,
            unitMask = unitMask
        };

        ScheduleAOEJob(aoe);
    }

    public void ApplyAOEHeal(Vector3 position, float range, int heal, int unitMask)
    {
        //Create arrays if they're not created
        if (!units.IsCreated)
            InitializeArrays();

        AOEJob aoe = new AOEJob()
        {
            units = units,
            affectedUnits = affectedUnits,
            aoePosition = position,
            range = range,
            heal = heal,
            unitMask = unitMask
        };

        ScheduleAOEJob(aoe);
    }

    private void LateUpdate()
    {
        if (units.IsCreated)
            ExecuteJobs();
    }

    private void ScheduleAOEJob(AOEJob aoe)
    {
        //Make the current job dependant from the previously scheduled jobs 
        int count = aoeJobHandles.Length;
        JobHandle dependency = count == 0 ? default : aoeJobHandles[count - 1];
        JobHandle aoeHandle = aoe.Schedule(unitsInfo.Count, 75, dependency);
        aoeJobHandles.Add(aoeHandle);
    }

    private void ExecuteJobs()
    {
        JobHandle.CompleteAll(aoeJobHandles);

        foreach (AOEAffectedUnit unit in affectedUnits)
        {
            HealthController currentController = activeUnits[unit.unitID];

            if (currentController == null)
                continue;

            if (unit.heal > 0)
                currentController.Heal(unit.heal);

            if (unit.damage > 0)
                currentController.TakeHit(unit.damage);
        }

        DisposeArrays();
    }

    private void InitializeArrays()
    {
        aoeJobHandles = new NativeList<JobHandle>(10, Allocator.Persistent);
        units = unitsInfo.GetJobArray();
        affectedUnits = new NativeArray<AOEAffectedUnit>(unitsInfo.Count, Allocator.TempJob);

        //Initialize units damage to 0
        for (int i = 0; i < units.Length; i++)
        {
            affectedUnits[i] = new AOEAffectedUnit()
            {
                unitID = units[i].instanceID,
                damage = 0
            };
        }
    }

    private void DisposeArrays()
    {
        units.Dispose();
        affectedUnits.Dispose();
        aoeJobHandles.Dispose();
    }

    private void OnDestroy()
    {
        if (units.IsCreated)
            DisposeArrays();
    }
}