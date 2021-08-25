using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct DistanceDetectionJob : IJobParallelFor
{
    public NativeArray<UnitInfo> unitInfo;
    [ReadOnly] public NativeArray<UnitInfo> othersInfo;

    public void Execute(int index)
    {
        //Get current unit info and position
        UnitInfo currentUnit = unitInfo[index];
        float3 position = currentUnit.Position;
        float currentDistance = math.INFINITY;

        foreach (UnitInfo other in othersInfo)
        {
            if ((currentUnit.UnitMask & other.UnitTypeID) == 0)
                continue;

            if (currentUnit.InstanceID == other.InstanceID)
                continue;

            //Calculate distance from unit
            float3 otherPosition = other.Position;
            float temp = math.distance(position, otherPosition);

            //Update target and current distance
            if (temp < currentDistance)
            {
                currentUnit.TargetID = other.InstanceID;
                currentDistance = temp;
            }
        }

        //Update unit info inside dictionary
        unitInfo[index] = currentUnit;
    }
}

//Struct used to store info inside jobs
public struct UnitInfo
{
    public int InstanceID;
    public float3 Position;
    public int Health;
    public int TargetID;
    public int UnitTypeID;
    public int UnitMask;
}