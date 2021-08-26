using System.Collections;
using UnityEngine;

public class TroopPlacementManager : UnitPlacementManager
{
    public static TroopPlacementManager Instance { get; private set; }

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

        //Check that wave didn't start yet
        if (!CanPlaceUnits)
        {
            StopPlacement();
            yield break;
        }

        bool canPlaceUnit = true;

        do
        {
            yield return null;

            //Check that we hit the ground
            if (!CameraRay.GetCameraHitPoint(out Vector3 hitPoint, groundLayer))
                continue;

            //Make sure units are not colliding
            if (IsUnitColliding(hitPoint))
                continue;

            SpawnUnit(hitPoint);

            ShopManager.Instance.BuyUnit(currentUnitPrice);

            //Check whether or not we reached the limit and can buy the next unit
            canPlaceUnit = CanAddUnits && ShopManager.Instance.CanBuyUnit(currentUnitPrice);

        } while (CanPlaceUnits && Input.GetMouseButton(0) && canPlaceUnit);

        StopPlacement();
    }

    private void SpawnUnit(Vector3 hitPoint)
    {
        GameObject troop = PoolManager.Instance.GetPooledObject(currentUnitID, hitPoint);
        troop.SetActive(true);
    }
}