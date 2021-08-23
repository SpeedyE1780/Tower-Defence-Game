public class BuyTowerController : BuyUnitController
{
    protected override void SpawnUnit() => TowerPlacementManager.Instance.UpdateCurrentUnit(unitID, unitPrice);
}