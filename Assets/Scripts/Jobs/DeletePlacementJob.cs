using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct DeletePlacementJob : IJobParallelFor
{
    //Pass info from job to manager in the first element
    [NativeDisableParallelForRestriction] public NativeArray<DeletedUnitInfo> deletedInfo;
    [ReadOnly] public NativeArray<UnitInfo> units;
    public float3 position;
    public float distanceThreshold;

    public void Execute(int index)
    {
        //Delete last unit that was in range of mouse position
        if (math.distance(position, units[index].position) < distanceThreshold)
        {
            DeletedUnitInfo info = new DeletedUnitInfo()
            {
                found = true,
                id = units[index].instanceID
            };

            deletedInfo[0] = info;
        }
    }
}

public struct DeletedUnitInfo
{
    public int id;
    public bool found;
}
