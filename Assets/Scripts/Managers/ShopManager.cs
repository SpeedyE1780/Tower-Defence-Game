using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int startingCoins;

    private int currentCoins;

    private void OnEnable() => EventManager.OnEnemyKilled += AddCurrency;
    private void OnDisable() => EventManager.OnEnemyKilled -= AddCurrency;
    private void Start() => UpdateCurrency(startingCoins);
    private void AddCurrency(int points, int coins) => UpdateCurrency(coins);
    public bool CanBuyUnit(int price) => currentCoins >= price;
    public void BuyUnit(int price) => UpdateCurrency(-price);

    private void UpdateCurrency(int amount)
    {
        currentCoins += amount;
        UIManager.Instance.UpdateCurrencyText(currentCoins);
    }
}