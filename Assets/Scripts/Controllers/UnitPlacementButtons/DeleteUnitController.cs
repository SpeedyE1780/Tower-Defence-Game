using UnityEngine;

public class DeleteUnitController : MonoBehaviour
{
    public void StartDeleteUnit()
    {
        if (PlacementManager.IsPlacingUnits)
            return;

        DeleteUnitManager.Instance.StartDeleteUnit();
    }
}