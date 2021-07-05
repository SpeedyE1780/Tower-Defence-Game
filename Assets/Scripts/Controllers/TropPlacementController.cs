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

    public void StartUnitPlacement(GameObject unit, int unitPrice)
    {
        SetHighlightedAreaState(true);
        StartCoroutine(PlaceUnit(unit, unitPrice));
    }

    private IEnumerator PlaceUnit(GameObject unit, int unitPrice)
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

            Instantiate(unit, hitPoint, Quaternion.identity);
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