using UnityEngine;
using UnityEngine.UI;

public class BuyTowerController : MonoBehaviour
{
    [SerializeField] private Image towerIcon;
    [SerializeField] private Text priceText;
    [SerializeField] private Sprite towerSprite;
    [SerializeField] private int price;
    [SerializeField] private TowerShootingController tower;

    private void Awake()
    {
        towerIcon.sprite = towerSprite;
        priceText.text = price.ToString();
    }

    public void BuyTower()
    {
        ShopManager.Instance.BuyTower(price, tower);
    }
}