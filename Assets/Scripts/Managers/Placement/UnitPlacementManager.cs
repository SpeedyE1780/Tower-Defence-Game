using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public abstract class UnitPlacementManager : PlacementManager
{
    public const int MaximumUnits = 150;

    [SerializeField] protected float distanceBetweenUnits;
    [SerializeField] UnitsInfoSet unitsInfo;
    [SerializeField] protected LayerMask groundLayer;
    protected PoolID currentUnitID;
    protected int currentUnitPrice;

    //Currently placed units
    public static int UnitCount { get; private set; }

    //Checks if we reached the units limits or not
    public static bool CanAddUnits => UnitCount < MaximumUnits;

    public static void ResetUnitCount() => UnitCount = 0;
    public static void RaiseUnitCount() => UnitCount += 1;
    public static void LowerUnitCount() => UnitCount -= 1;

    protected bool IsUnitColliding(Vector3 position)
    {
        //Initialize Arrays
        NativeArray<UnitInfo> units = unitsInfo.GetJobArray();
        NativeArray<bool> unitsColliding = new NativeArray<bool>(new bool[] { false }, Allocator.TempJob);

        UnitPlacementJob unitPlacement = new UnitPlacementJob()
        {
            unitsInfo = units,
            minimumDistance = distanceBetweenUnits,
            position = position,
            invalidPosition = unitsColliding
        };

        //Complete job
        unitPlacement.Schedule(unitsInfo.Count, 75).Complete();

        //Save outpput then dispose arrays
        bool output = unitsColliding[0];
        units.Dispose();
        unitsColliding.Dispose();

        return output;
    }

    public void UpdateCurrentUnit(PoolID unitID, int unitPrice)
    {
        currentUnitID = unitID;
        currentUnitPrice = unitPrice;
        StartPlacement();
    }
}