using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    private void OnEnable() => UnitPlacementManager.RaiseUnitCount();
    private void OnDisable() => UnitPlacementManager.LowerUnitCount();
}