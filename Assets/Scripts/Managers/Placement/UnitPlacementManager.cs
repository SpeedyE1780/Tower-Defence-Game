using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitPlacementManager : PlacementManager
{
    protected const int MaximumUnits = 150;

    [SerializeField] protected float distanceBetweenUnits;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask unitLayers;
    [SerializeField] protected List<GameObject> highlightedArea;

    public static int UnitCount { get; private set; }
    public static bool CanAddUnits => UnitCount < MaximumUnits;

    public static void ResetUnitCount() => UnitCount = 0;
    public static void RaiseUnitCount() => UnitCount += 1;
    public static void LowerUnitCount() => UnitCount -= 1;

    public void StartUnitPlacement(PoolID troopID, int unitPrice)
    {
        SetHighlightedAreaState(true);
        StartCoroutine(PlaceUnit(troopID, unitPrice));
        CanPlaceUnits = false;
    }

    protected void StopUnitPlacement()
    {
        SetHighlightedAreaState(false);
        CanPlaceUnits = true;
    }

    protected abstract IEnumerator PlaceUnit(PoolID troopID, int unitPrice);

    private void SetHighlightedAreaState(bool state)
    {
        foreach (GameObject area in highlightedArea)
            area.SetActive(state);
    }
}