using System.Collections;
using UnityEngine;

public class CatapultPlacementManager : UnitPlacementManager
{
    public static CatapultPlacementManager Instance { get; private set; }

    [SerializeField] private Transform defaultCatapultPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    protected override IEnumerator ProcessPlacement()
    {
        CatapultController catapult = PoolManager.Instance.GetPooledObject<CatapultController>(currentUnitID);
        catapult.gameObject.SetActive(true);
        catapult.enabled = false;
        Transform towerTransform = catapult.transform;
        towerTransform.position = defaultCatapultPosition.position;

        //Wait for finger drag
        yield return waitForDrag;

        while (CanPlaceUnits)
        {
            yield return null;

            if (!CameraRay.GetCameraHitPoint(out Vector3 hitPoint, groundLayer))
            {
                //Stop placing if finger released outside of placement area
                if (Input.GetMouseButtonUp(0))
                    break;
                else
                    continue;
            }

            towerTransform.position = hitPoint;

            //If finger released and no unit is colliding with tower place it
            if (Input.GetMouseButtonUp(0))
            {
                if (IsUnitColliding(hitPoint))
                    continue;

                catapult.enabled = true;
                ShopManager.Instance.BuyUnit(currentUnitPrice);
                catapult = null;
                break;
            }
        }

        //Pool unit if no valid position was found
        if (catapult != null)
            PoolTower(catapult);

        StopPlacement();
    }

    private void PoolTower(CatapultController catapult) => PoolManager.Instance.AddToPool(currentUnitID, catapult.gameObject);
}
