using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct FindClosestUnitJob : IJobParallelFor
{
    public NativeArray<UnitInfo> unitInfo;
    [ReadOnly] public NativeArray<UnitInfo> othersInfo;

    public void Execute(int index)
    {
        //Get current unit info and position
        UnitInfo currentUnit = unitInfo[index];
        float3 position = currentUnit.position;
        float currentDistance = math.INFINITY;

        foreach (UnitInfo other in othersInfo)
        {
            if (!currentUnit.CanTarget(other.instanceID, other.unitTypeID))
                continue;

            //Calculate distance from unit
            float3 otherPosition = other.position;
            float temp = math.distance(position, otherPosition);

            //Update target and current distance
            if (temp < currentDistance)
            {
                currentUnit.targetID = other.instanceID;
                currentDistance = temp;
            }
        }

        //Update unit info inside dictionary
        unitInfo[index] = currentUnit;
    }
}
