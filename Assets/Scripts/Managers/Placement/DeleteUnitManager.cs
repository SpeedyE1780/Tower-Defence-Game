using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class DeleteUnitManager : PlacementManager
{
    public static DeleteUnitManager Instance { get; private set; }

    [SerializeField] ActiveUnitsSet activeUnits;
    [SerializeField] UnitsInfoSet unitsInfo;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int distanceThreshold;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    protected override IEnumerator ProcessPlacement()
    {
        //Wait for finger drag or wave to start
        yield return waitForDrag;

        //Delete any units we hovered over while dragging our finger while waiting for the wave to start
        while (CanPlaceUnits && Input.GetMouseButton(0))
        {
            yield return null;

            if (!CameraRay.GetCameraHitPoint(out Vector3 point, groundLayer))
                continue;

            //Initialize arrays
            NativeArray<UnitInfo> units = unitsInfo.GetJobArray();
            NativeArray<int> targetID = new NativeArray<int>(new int[] { 1 }, Allocator.TempJob);

            DeletePlacementJob deletePlacement = new DeletePlacementJob()
            {
                Units = units,
                TargetUnit = targetID,
                DistanceThreshold = distanceThreshold,
                Position = point
            };

            deletePlacement.Run(unitsInfo.Count);
            int target = targetID[0];

            if (target < 0)
                activeUnits[target].GetComponent<UnitController>().PoolUnit();

            //Dispose arrays
            units.Dispose();
            targetID.Dispose();
        }

        StopPlacement();
    }
}