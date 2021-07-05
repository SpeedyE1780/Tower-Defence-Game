using UnityEngine;

public class UnitPlacementManager : Singleton<UnitPlacementManager>
{
    [SerializeField] private TowerPlacementController towerPlacement;
    [SerializeField] private TropPlacementController unitPlacement;

    public void PlaceTower(TowerShootingController tower)
    {
        towerPlacement.StartTowerPlacement(tower);
    }

    public void PlaceTroop(GameObject troop, int unitPrice)
    {
        unitPlacement.StartUnitPlacement(troop, unitPrice);
    }
}