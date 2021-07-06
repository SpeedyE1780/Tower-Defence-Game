using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitPlacementManager : Singleton<UnitPlacementManager>
{
    [SerializeField] protected float distanceBetweenUnits;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask unitLayers;
    [SerializeField] protected List<GameObject> highlightedArea;

    public static bool CanPlaceUnits { get; private set; }

    private void Start() => CanPlaceUnits = true;

    public void StartUnitPlacement(PoolID troopID, int unitPrice)
    {
        SetHighlightedAreaState(true);
        StartCoroutine(PlaceUnit(troopID, unitPrice));
        CanPlaceUnits = false;
    }

    protected void StopUnitPlacement()
    {
        SetHighlightedAreaState(false);
        CanPlaceUnits = false;
    }

    protected abstract IEnumerator PlaceUnit(PoolID troopID, int unitPrice);

    private void SetHighlightedAreaState(bool state)
    {
        foreach (GameObject area in highlightedArea)
            area.SetActive(state);
    }
}