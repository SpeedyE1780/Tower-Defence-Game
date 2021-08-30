using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct DeletePlacementJob : IJobParallelFor
{
    [NativeDisableParallelForRestriction] public NativeArray<int> TargetUnit;
    [ReadOnly] public NativeArray<UnitInfo> Units;
    public float3 Position;
    public float DistanceThreshold;

    public void Execute(int index)
    {
        if (math.distance(Position, Units[index].Position) < DistanceThreshold)
            TargetUnit[0] = Units[index].InstanceID;
    }
}