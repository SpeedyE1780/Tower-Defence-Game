using System.Collections;
using UnityEngine;

public class DeleteUnitManager : PlacementManager
{
    public static DeleteUnitManager Instance { get; private set; }

    [SerializeField] private LayerMask unitsLayer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    protected override IEnumerator ProcessPlacement()
    {
        //Wait for finger drag
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        //Delete any units we hovered over while dragging our finger while waiting for the wave to start
        while (CanPlaceUnits && Input.GetMouseButton(0))
        {
            if (CameraRay.GetCameraHitUnit(out Transform unit, unitsLayer))
                Destroy(unit.gameObject);

            yield return null;
        }

        StopPlacement();
    }
}