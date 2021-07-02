using System.Collections;
using UnityEngine;

public class UnitPlacementController : MonoBehaviour
{
    [SerializeField] private float distanceBetweenUnits;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask unitLayers;
    [SerializeField] private Camera mainCamera;

    public void StartUnitPlacement(GameObject unit) => StartCoroutine(PlaceUnit(unit));

    private IEnumerator PlaceUnit(GameObject unit)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        while (Input.GetMouseButton(0))
        {
            yield return null;

            if (!CameraRay.GetCameraHitPoint(out Vector3 hitPoint, groundLayer))
                continue;

            if (Physics.CheckSphere(hitPoint, distanceBetweenUnits, unitLayers, QueryTriggerInteraction.Ignore))
                continue;

            Instantiate(unit, hitPoint, Quaternion.identity);
        }
    }
}