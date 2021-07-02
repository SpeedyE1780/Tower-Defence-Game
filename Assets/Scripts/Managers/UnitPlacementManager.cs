using UnityEngine;

public class UnitPlacementManager : Singleton<UnitPlacementManager>
{
    [SerializeField] private TowerPlacementController towerPlacement;
    [SerializeField] private UnitPlacementController unitPlacement;

    public void PlaceTower(TowerShootingController tower)
    {
        towerPlacement.StartTowerPlacement(tower);
    }

    public void PlaceUnit(GameObject unit, int unitPrice)
    {
        unitPlacement.StartUnitPlacement(unit, unitPrice);
    }
}