using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct FindMostInjuredUnitJob : IJobParallelFor
{
    private const float HealthFactor = 0.65f;
    private const float DistanceFactor = 0.35f;

    public NativeArray<UnitInfo> unitInfo;
    [ReadOnly] public NativeArray<UnitInfo> othersInfo;

    public void Execute(int index)
    {
        //Get current unit info
        UnitInfo currentUnit = unitInfo[index];
        float currentFactor = math.INFINITY;

        foreach (UnitInfo other in othersInfo)
        {
            if (!currentUnit.CanAttack(other.InstanceID, other.UnitTypeID))
                continue;

            //Get how far the target is and how injured he is
            float healthFactor = other.HealthPercentage * HealthFactor;
            float distanceFactor = math.distance(currentUnit.Position, other.Position) * DistanceFactor;
            float tempFactor = healthFactor + distanceFactor;

            //Update target and factor
            if (tempFactor < currentFactor)
            {
                currentUnit.TargetID = other.InstanceID;
                currentFactor = tempFactor;
            }
        }

        //Update unit info inside dictionary
        unitInfo[index] = currentUnit;
    }
}