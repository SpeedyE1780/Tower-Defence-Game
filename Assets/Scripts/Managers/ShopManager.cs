using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int startingCoins;

    private int currentCoins;

    private void Start() => UpdateCurrency(startingCoins);
    public bool CanBuyUnit(int price) => currentCoins >= price;
    public void BuyUnit(int price) => UpdateCurrency(-price);

    public void UpdateCurrency(int amount)
    {
        currentCoins += amount;
        UIManager.Instance.UpdateCurrencyText(currentCoins);
    }
}