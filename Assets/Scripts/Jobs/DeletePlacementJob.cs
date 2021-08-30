using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct DeletePlacementJob : IJobParallelFor
{
    [NativeDisableParallelForRestriction] public NativeArray<DeletedUnitInfo> DeletedInfo;
    [ReadOnly] public NativeArray<UnitInfo> Units;
    public float3 Position;
    public float DistanceThreshold;

    public void Execute(int index)
    {
        if (math.distance(Position, Units[index].Position) < DistanceThreshold)
        {
            DeletedUnitInfo info = new DeletedUnitInfo()
            {
                Found = true,
                ID = Units[index].InstanceID
            };

            DeletedInfo[0] = info;
        }
    }
}

public struct DeletedUnitInfo
{
    public int ID;
    public bool Found;
}