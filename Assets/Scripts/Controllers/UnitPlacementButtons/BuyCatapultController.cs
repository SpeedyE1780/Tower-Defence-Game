public class BuyCatapultController : BuyUnitController
{
    protected override void SpawnUnit() => CatapultPlacementManager.Instance.UpdateCurrentUnit(unitID, unitPrice);
}
