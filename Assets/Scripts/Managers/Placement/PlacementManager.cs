using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlacementManager : MonoBehaviour
{
    [SerializeField] protected List<GameObject> highlightedAreas;

    //Checks if we are currently placing or deleting units
    public static bool IsPlacingUnits { get; protected set; }
    public static bool CanPlaceUnits { get; private set; }

    protected WaitUntil waitForDrag;

    [RuntimeInitializeOnLoadMethod]
    private static void SetIsPlacingUnits() => IsPlacingUnits = false;
    public static void SetCanPlaceUnits(bool state) => CanPlaceUnits = state;

    private void Start()
    {
        //Wait for finger drag or wave to start
        waitForDrag = new WaitUntil(() => Input.GetMouseButtonDown(0) || !CanPlaceUnits);
    }

    public virtual void StartPlacement()
    {
        if (IsPlacingUnits || !CanPlaceUnits)
            return;

        //Highlight areas where units can be deleted and start deleting units
        StartCoroutine(ProcessPlacement());
        ToggleHighlightedAreas(true);
        UIManager.Instance.ToggleUnitPlacementCanvas(false);
        IsPlacingUnits = true;
        waitForDrag.Reset();
    }

    public virtual void StopPlacement()
    {
        ToggleHighlightedAreas(false);
        UIManager.Instance.ToggleUnitPlacementCanvas(true);
        IsPlacingUnits = false;
    }

    protected void ToggleHighlightedAreas(bool state)
    {
        foreach (GameObject area in highlightedAreas)
            area.SetActive(state);
    }

    protected abstract IEnumerator ProcessPlacement();
}