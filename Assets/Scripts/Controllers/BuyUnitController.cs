using UnityEngine;
using UnityEngine.UI;

public abstract class BuyUnitController : MonoBehaviour
{
    [SerializeField] protected Image unitIcon;
    [SerializeField] protected Text priceText;
    [SerializeField] protected Sprite unitSprite;
    [SerializeField] protected int unitPrice;
    [SerializeField] protected PoolID unitID;

    protected virtual void Awake()
    {
        unitIcon.sprite = unitSprite;
        priceText.text = unitPrice.ToString();
    }

    public void BuyUnit()
    {
        if (!ShopManager.Instance.CanBuyUnit(unitPrice) || !PlacementManager.CanPlaceUnits)
            return;

        SpawnUnit();
    }

    protected abstract void SpawnUnit();
}