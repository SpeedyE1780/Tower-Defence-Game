using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int startingCoins;

    private int currentCoins;

    private void Start()
    {
        currentCoins = 0;
        UpdateCurrency(startingCoins);
    }

    private void OnEnable()
    {
        EventManager.OnEnemyKilled += AddCurrency;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyKilled -= AddCurrency;
    }

    private void AddCurrency(int points, int coins)
    {
        UpdateCurrency(coins);
    }

    private void UpdateCurrency(int amount)
    {
        currentCoins += amount;
        UIManager.Instance.UpdateCurrencyText(currentCoins);
    }

    public void BuyTower(int price, PoolID tower)
    {
        if (currentCoins < price || !UnitPlacementManager.Instance.CanPlaceUnits)
            return;

        UpdateCurrency(-price);
        UnitPlacementManager.Instance.PlaceTower(tower);
    }

    public void BuyUnits(int price, GameObject unit)
    {
        if (currentCoins < price || !UnitPlacementManager.Instance.CanPlaceUnits)
            return;

        UnitPlacementManager.Instance.PlaceTroop(unit, price);
    }

    public bool BoughtUnit(int price)
    {
        if (currentCoins < price)
            return false;

        UpdateCurrency(-price);
        return true;
    }
}