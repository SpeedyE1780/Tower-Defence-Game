public class BuyTroopController : BuyUnitController
{
    protected override void SpawnUnit()
    {
        TroopPlacementManager.Instance.StartUnitPlacement(unitID, unitPrice);
    }
}