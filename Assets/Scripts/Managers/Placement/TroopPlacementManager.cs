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

    protected override IEnumerator PlaceUnit(PoolID troopID, int unitPrice)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        bool canPlaceUnit = true;

        do
        {
            yield return null;

            if (!CameraRay.GetCameraHitPoint(out Vector3 hitPoint, groundLayer))
                continue;

            if (Physics.CheckSphere(hitPoint, distanceBetweenUnits, unitLayers, QueryTriggerInteraction.Ignore))
                continue;

            GameObject troop = PoolManager.Instance.GetPooledObject(troopID);
            troop.transform.SetPositionAndRotation(hitPoint, Quaternion.identity);
            ShopManager.Instance.BuyUnit(unitPrice);
            canPlaceUnit = ShopManager.Instance.CanBuyUnit(unitPrice);

            if (troopID == PoolID.Infantry)
                troop.GetComponent<InfantryController>().initialPosition = hitPoint;

        } while (Input.GetMouseButton(0) && canPlaceUnit);

        StopUnitPlacement();
    }
}