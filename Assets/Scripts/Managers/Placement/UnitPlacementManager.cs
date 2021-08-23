using UnityEngine;

public abstract class UnitPlacementManager : PlacementManager
{
    public const int MaximumUnits = 150;

    [SerializeField] protected float distanceBetweenUnits;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask unitLayers;
    protected PoolID currentUnitID;
    protected int currentUnitPrice;

    //Currently placed units
    public static int UnitCount { get; private set; }

    //Checks if we reached the units limits or not
    public static bool CanAddUnits => UnitCount < MaximumUnits;

    public static void ResetUnitCount() => UnitCount = 0;
    public static void RaiseUnitCount() => UnitCount += 1;
    public static void LowerUnitCount() => UnitCount -= 1;

    public void UpdateCurrentUnit(PoolID unitID, int unitPrice)
    {
        currentUnitID = unitID;
        currentUnitPrice = unitPrice;
        StartPlacement();
    }
}