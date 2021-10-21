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

    //Wait for finger drag or wave to start
    private void Start() => waitForDrag = new WaitUntil(() => Input.GetMouseButtonDown(0) || !CanPlaceUnits);

    public virtual void StartPlacement()
    {
        if (IsPlacingUnits || !CanPlaceUnits)
            return;

        waitForDrag.Reset();
        StartCoroutine(ProcessPlacement());
        UpdatePlacementValues(true);
    }

    public virtual void StopPlacement() => UpdatePlacementValues(false);

    private void UpdatePlacementValues(bool isPlacing)
    {
        IsPlacingUnits = isPlacing;
        ToggleHighlightedAreas(isPlacing);

        //Check with can place units to prevent enabling placement UI when wave starts in case stop placement is called on wave start
        UIManager.Instance.ToggleUnitPlacementCanvas(!isPlacing && CanPlaceUnits);
    }

    //Highlight areas where units can be placed/deleted
    protected void ToggleHighlightedAreas(bool state)
    {
        foreach (GameObject area in highlightedAreas)
            area.SetActive(state);
    }

    protected abstract IEnumerator ProcessPlacement();
}
