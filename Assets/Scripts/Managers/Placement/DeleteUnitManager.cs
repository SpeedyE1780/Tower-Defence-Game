using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUnitManager : PlacementManager
{
    public static DeleteUnitManager Instance { get; private set; }

    [SerializeField] private LayerMask unitsLayer;
    [SerializeField] private List<GameObject> highlightedAreas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartDeleteUnit()
    {
        if (IsPlacingUnits)
            return;

        StartCoroutine(DeleteUnit());
        SetHighlightedAreaState(true);
        IsPlacingUnits = true;
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
        IsPlacingUnits = false;
    }

    private void SetHighlightedAreaState(bool state)
    {
        foreach (GameObject area in highlightedAreas)
            area.SetActive(state);
    }
}