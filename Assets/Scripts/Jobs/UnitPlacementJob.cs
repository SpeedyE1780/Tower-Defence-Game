using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct UnitPlacementJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<UnitInfo> unitsInfo;
    public float3 Position;
    public float MinimumDistance;
    [NativeDisableParallelForRestriction] public NativeArray<bool> InvalidPosition;

    public void Execute(int index)
    {
        float3 unitPosition = unitsInfo[index].Position;

        if (math.distance(Position, unitPosition) < MinimumDistance)
            InvalidPosition[0] = true;
    }
}