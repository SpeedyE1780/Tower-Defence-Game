public class BuyTroopController : BuyUnitController
{
    public override void BuyUnit()
    {
        ShopManager.Instance.BuyUnits(unitPrice, unitID);
    }
}