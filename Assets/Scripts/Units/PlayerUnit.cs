using UnityEngine;

//Assigned to all player units raise current unit count
public class PlayerUnit : MonoBehaviour
{
    private void OnEnable() => UnitPlacementManager.RaiseUnitCount();
    private void OnDisable() => UnitPlacementManager.LowerUnitCount();
}
