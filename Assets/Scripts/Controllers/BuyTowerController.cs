public class BuyTowerController : BuyUnitController
{
    protected override void SpawnUnit() => TowerPlacementManager.Instance.StartUnitPlacement(unitID, unitPrice);
}