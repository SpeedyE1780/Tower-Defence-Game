using System.Collections;
using UnityEngine;

public class TowerPlacementController : MonoBehaviour
{
    [SerializeField] private Transform defaultTowerPosition;
    [SerializeField] private float checkRaduis;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private LayerMask groundLayer;

    public void StartTowerPlacement(TowerShootingController tower) => StartCoroutine(PlaceTower(tower));

    IEnumerator PlaceTower(TowerShootingController tower)
    {
        tower = Instantiate(tower);
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
                if (Physics.OverlapSphere(towerTransform.position, checkRaduis, towerLayer).Length > 1)
                    continue;

                tower.enabled = true;
                yield break;
            }
        }
    }
}