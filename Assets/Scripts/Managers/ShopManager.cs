using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int startingCoins;
    [SerializeField] private int unitPrice;

    private int currentCoins;

    private void Start()
    {
        currentCoins = startingCoins;
        UIManager.Instance.UpdateCurrencyText(currentCoins);
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
        currentCoins += coins;
        UIManager.Instance.UpdateCurrencyText(currentCoins);
    }

    public void BuyTower(int price, TowerShootingController tower)
    {
        if (currentCoins < price)
            return;

        currentCoins -= price;
        UIManager.Instance.UpdateCurrencyText(currentCoins);
        UnitPlacementManager.Instance.PlaceTower(tower);
    }

    public void BuyUnit(int price, GameObject unit)
    {
        
    }
}