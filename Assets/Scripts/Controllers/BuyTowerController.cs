public class BuyTowerController : BuyUnitController
{
    public override void BuyUnit()
    {
        ShopManager.Instance.BuyTower(unitPrice, unitID);
    }
}