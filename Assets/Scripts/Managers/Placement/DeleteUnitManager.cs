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

            //Initialize job variables
            DeletedUnitInfo info = new DeletedUnitInfo() { found = false };
            NativeArray<UnitInfo> units = unitsInfo.GetJobArray();
            NativeArray<DeletedUnitInfo> deletedInfo = new NativeArray<DeletedUnitInfo>(new DeletedUnitInfo[] { info }, Allocator.TempJob);

            DeletePlacementJob deletePlacement = new DeletePlacementJob()
            {
                units = units,
                deletedInfo = deletedInfo,
                distanceThreshold = distanceThreshold,
                position = point
            };

            //Run and update job
            deletePlacement.Run(unitsInfo.Count);
            info = deletedInfo[0];

            //If a unit was found delete it
            if (info.found)
                activeUnits[info.id].GetComponent<UnitController>().PoolUnit();

            //Dispose arrays
            units.Dispose();
            deletedInfo.Dispose();
        }

        StopPlacement();
    }
}