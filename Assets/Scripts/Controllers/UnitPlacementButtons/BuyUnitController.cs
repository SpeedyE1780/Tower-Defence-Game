using UnityEngine;
using UnityEngine.UI;

public abstract class BuyUnitController : MonoBehaviour
{
    [SerializeField] protected Image unitIcon;
    [SerializeField] protected Text priceText;
    [SerializeField] protected Sprite unitSprite;
    [SerializeField] protected PoolID unitID;
    [SerializeField] protected int unitPrice;

    private void Awake() => SetButtonVisuals();

    protected void SetButtonVisuals()
    {
        //Set button sprite and text
        unitIcon.sprite = unitSprite;
        priceText.text = unitPrice.ToString();
    }

    public void BuyUnit()
    {
        //Check if we  can buy a unit and place it
        if (!ShopManager.Instance.CanBuyUnit(unitPrice) || PlacementManager.IsPlacingUnits || !UnitPlacementManager.CanAddUnits)
            return;

        SpawnUnit();
    }

    protected abstract void SpawnUnit();
}
