using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct UnitPlacementJob : IJobParallelFor
{
    //Pass info from job to manager in the first element
    [ReadOnly] public NativeArray<UnitInfo> unitsInfo;
    public float3 position;
    public float minimumDistance;
    [NativeDisableParallelForRestriction] public NativeArray<bool> invalidPosition;

    public void Execute(int index)
    {
        float3 unitPosition = unitsInfo[index].position;

        //If unit is too close to mouse position then its invalid
        if (math.distance(position, unitPosition) < minimumDistance)
            invalidPosition[0] = true;
    }
}
