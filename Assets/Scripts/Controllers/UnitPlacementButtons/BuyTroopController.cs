public class BuyTroopController : BuyUnitController
{
    protected override void SpawnUnit() => TroopPlacementManager.Instance.UpdateCurrentUnit(unitID, unitPrice);
}
