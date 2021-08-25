using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct HealerDetectionJob : IJobParallelFor
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
            if ((currentUnit.UnitMask & other.UnitTypeID) == 0)
                continue;

            if (currentUnit.InstanceID == other.InstanceID)
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
