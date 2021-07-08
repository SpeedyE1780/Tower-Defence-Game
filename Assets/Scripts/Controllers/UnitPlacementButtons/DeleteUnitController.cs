using UnityEngine;

public class DeleteUnitController : MonoBehaviour
{
    public void StartDeleteUnit()
    {
        if (!PlacementManager.CanPlaceUnits)
            return;

        DeleteUnitManager.Instance.StartDeleteUnit();
    }
}