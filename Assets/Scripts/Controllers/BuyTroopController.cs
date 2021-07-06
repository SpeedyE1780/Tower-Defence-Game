using UnityEngine;
using UnityEngine.UI;

public class BuyTroopController : MonoBehaviour
{
    [SerializeField] private Image unitIcon;
    [SerializeField] private Text priceText;
    [SerializeField] private Sprite unitSprite;
    [SerializeField] private int price;
    [SerializeField] private PoolID troopID;

    private void Awake()
    {
        unitIcon.sprite = unitSprite;
        priceText.text = price.ToString();
    }

    public void BuyUnit()
    {
        ShopManager.Instance.BuyUnits(price, troopID);
    }
}