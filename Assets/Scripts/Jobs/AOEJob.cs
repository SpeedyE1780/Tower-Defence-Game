using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct AOEJob : IJobParallelFor
{
    public NativeArray<AOEDamagedUnit> affectedUnits;
    [ReadOnly] public NativeArray<UnitInfo> units;
    public float3 aoePosition;
    public float range;
    public int damage;
    public int heal;
    public int unitMask;

    public void Execute(int index)
    {
        //Get distance from blast
        UnitInfo current = units[index];
        float distance = math.distance(aoePosition, current.Position);

        if ((unitMask & current.UnitTypeID) == 0)
            return;

        if (distance > range)
            return;

        //Get damage/heal from current job
        float multiplier = 1 - distance / range;
        int unitDamage = (int)math.round(multiplier * damage);
        int unitHeal = (int)math.round(multiplier * heal);

        //Add damage/heal from current job to the damage received from the previous jobs
        AOEDamagedUnit unit = affectedUnits[index];
        unit.Damage += unitDamage;
        unit.Heal += unitHeal;
        affectedUnits[index] = unit;
    }
}

public struct AOEDamagedUnit
{
    public int UnitID;
    public int Damage;
    public int Heal;
}