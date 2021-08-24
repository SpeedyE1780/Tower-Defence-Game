using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct DetectionJob : IJobParallelFor
{
    public NativeArray<UnitInfo> unitInfo;
    [ReadOnly] public NativeArray<UnitInfo> enemyInfo;

    public void Execute(int index)
    {
        //Get current unit info and position
        UnitInfo currentUnit = unitInfo[index];
        float3 position = currentUnit.Position;
        float currentDistance = math.INFINITY;

        foreach (UnitInfo enemy in enemyInfo)
        {
            if ((currentUnit.UnitMask & enemy.UnitTypeID) == 0)
                continue;

            if (currentUnit.InstanceID == enemy.InstanceID)
                continue;

            //Calculate distance from unit
            float3 enemyPosition = enemy.Position;
            float temp = math.distance(position, enemyPosition);

            //Update target and current distance
            if (temp < currentDistance)
            {
                currentUnit.TargetID = enemy.InstanceID;
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
    public int TargetID;
    public int UnitTypeID;
    public int UnitMask;
}