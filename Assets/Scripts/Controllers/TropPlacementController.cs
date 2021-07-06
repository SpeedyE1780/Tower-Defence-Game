using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropPlacementController : MonoBehaviour
{
    [SerializeField] private float distanceBetweenUnits;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask unitLayers;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<GameObject> highlightedArea;

    public void StartUnitPlacement(PoolID troopID, int unitPrice)
    {
        SetHighlightedAreaState(true);
        StartCoroutine(PlaceUnit(troopID, unitPrice));
    }

    private IEnumerator PlaceUnit(PoolID troopID, int unitPrice)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        while (Input.GetMouseButton(0))
        {
            yield return null;

            if (!CameraRay.GetCameraHitPoint(out Vector3 hitPoint, groundLayer))
                continue;

            if (Physics.CheckSphere(hitPoint, distanceBetweenUnits, unitLayers, QueryTriggerInteraction.Ignore))
                continue;

            if (!ShopManager.Instance.BoughtUnit(unitPrice))
                break;

            GameObject troop = PoolManager.Instance.GetPooledObject(troopID);
            troop.transform.SetPositionAndRotation(hitPoint, Quaternion.identity);
        }

        SetHighlightedAreaState(false);
        UnitPlacementManager.Instance.SetCanPlaceUnits(true);
    }

    private void SetHighlightedAreaState(bool state)
    {
        foreach (GameObject area in highlightedArea)
            area.SetActive(state);
    }
}