using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int startingCoins;
    [SerializeField] private int maxCoins;
    private int currentCoins;

    public bool CanBuyUnit(int price) => currentCoins >= price;
    public void BuyUnit(int price) => UpdateCurrency(-price);
    public void SetCurrency() => UpdateCurrency(startingCoins);
    public void UpdateCurrency(int amount)
    {
        currentCoins = Mathf.Clamp(currentCoins + amount, 0, maxCoins);
        UIManager.Instance.UpdateCurrencyText(currentCoins);
    }
}