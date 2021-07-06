using System.Collections;
using UnityEngine;

public class TropPlacementController : UnitPlacementManager
{
    protected override IEnumerator PlaceUnit(PoolID troopID, int unitPrice)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        bool canPlaceUnit = true;

        do
        {
            if (!CameraRay.GetCameraHitPoint(out Vector3 hitPoint, groundLayer))
                continue;

            if (Physics.CheckSphere(hitPoint, distanceBetweenUnits, unitLayers, QueryTriggerInteraction.Ignore))
                continue;

            GameObject troop = PoolManager.Instance.GetPooledObject(troopID);
            troop.transform.SetPositionAndRotation(hitPoint, Quaternion.identity);
            ShopManager.Instance.BuyUnit(unitPrice);
            canPlaceUnit = ShopManager.Instance.CanBuyUnit(unitPrice);

            yield return null;

        } while (Input.GetMouseButton(0) && canPlaceUnit);

        StopUnitPlacement();
    }
}