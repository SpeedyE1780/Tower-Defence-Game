using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUnitManager : PlacementManager
{
    [SerializeField] private LayerMask unitsLayer;
    [SerializeField] private List<GameObject> highlightedAreas;


    public void StartDeleteUnit()
    {
        if (!CanPlaceUnits)
            return;
        StartCoroutine(DeleteUnit());
        SetHighlightedAreaState(true);
        CanPlaceUnits = false;
    }

    private IEnumerator DeleteUnit()
    {
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        while (Input.GetMouseButton(0))
        {
            if (CameraRay.GetCameraHitUnit(out Transform unit, unitsLayer))
                Destroy(unit.gameObject);

            yield return null;
        }

        StopDeleteUnit();
    }

    public void StopDeleteUnit()
    {
        SetHighlightedAreaState(false);
        CanPlaceUnits = true;
    }

    private void SetHighlightedAreaState(bool state)
    {
        foreach (GameObject area in highlightedAreas)
            area.SetActive(state);
    }
}