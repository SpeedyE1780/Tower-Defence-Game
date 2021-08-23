using System.Collections;
using UnityEngine;

public class TowerPlacementManager : UnitPlacementManager
{
    public static TowerPlacementManager Instance { get; private set; }

    [SerializeField] private Transform defaultTowerPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    protected override IEnumerator ProcessPlacement()
    {
        TowerController tower = PoolManager.Instance.GetPooledObject<TowerController>(currentUnitID);
        tower.gameObject.SetActive(true);
        tower.enabled = false;
        Transform towerTransform = tower.transform;
        towerTransform.position = defaultTowerPosition.position;

        yield return new WaitUntil(() => Input.GetMouseButton(0));

        while (true)
        {
            yield return null;

            if (!CameraRay.GetCameraHitPoint(out Vector3 hitPoint, groundLayer))
                continue;

            towerTransform.position = hitPoint;

            if (Input.GetMouseButtonUp(0))
            {
                if (Physics.OverlapSphere(towerTransform.position, distanceBetweenUnits, unitLayers).Length > 1)
                    continue;

                tower.enabled = true;
                break;
            }
        }

        ShopManager.Instance.BuyUnit(currentUnitPrice);
        StopPlacement();
    }
}