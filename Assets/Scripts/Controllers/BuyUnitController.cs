using UnityEngine;
using UnityEngine.UI;

public abstract class BuyUnitController : MonoBehaviour
{
    [SerializeField] private Image unitIcon;
    [SerializeField] private Text priceText;
    [SerializeField] private Sprite unitSprite;
    [SerializeField] private int unitPrice;
    [SerializeField] private PoolID unitID;

    protected virtual void Awake()
    {
        unitIcon.sprite = unitSprite;
        priceText.text = unitPrice.ToString();
    }

    public abstract void BuyUnit();
}