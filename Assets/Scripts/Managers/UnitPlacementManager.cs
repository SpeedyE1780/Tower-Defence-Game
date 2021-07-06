using UnityEngine;

public class UnitPlacementManager : Singleton<UnitPlacementManager>
{
    [SerializeField] private TowerPlacementController towerPlacement;
    [SerializeField] private TropPlacementController unitPlacement;
    public bool CanPlaceUnits { get; private set; }

    private void Start()
    {
        CanPlaceUnits = true;
    }

    public void PlaceTower(PoolID tower)
    {
        towerPlacement.StartTowerPlacement(tower);
        CanPlaceUnits = false;
    }

    public void PlaceTroop(PoolID troopID, int unitPrice)
    {
        unitPlacement.StartUnitPlacement(troopID, unitPrice);
        CanPlaceUnits = false;
    }

    public void SetCanPlaceUnits(bool state)
    {
        CanPlaceUnits = state;
    }
}