using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct FindMostInjuredUnitJob : IJobParallelFor
{
    public NativeArray<UnitInfo> unitInfo;
    [ReadOnly] public NativeArray<UnitInfo> othersInfo;

    public void Execute(int index)
    {
        //Get current unit info
        UnitInfo currentUnit = unitInfo[index];
        float maxHealth = math.INFINITY;

        foreach (UnitInfo other in othersInfo)
        {
            if (!currentUnit.CanAttack(other.InstanceID, other.UnitTypeID))
                continue;

            float temp = other.HealthPercentage;

            //Update target and current health percentage
            if (temp < maxHealth)
            {
                currentUnit.TargetID = other.InstanceID;
                maxHealth = temp;
            }
        }

        //Update unit info inside dictionary
        unitInfo[index] = currentUnit;
    }
}