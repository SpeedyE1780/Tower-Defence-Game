using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePlacementUnits : MonoBehaviour
{
    [SerializeField] Text price;
    [SerializeField] Transform areaTransform;
    [SerializeField] List<AreaUpgrade> areaUpgrades;
    private int currentUpgrade;
    private Vector3 maxScale;

    private void Awake()
    {
        maxScale = areaTransform.localScale;
        ResetArea();
        EventManager.OnGameRestarted += ResetArea;
    }

    public void UpgradeArea()
    {
        if (!ShopManager.Instance.CanBuyUnit(areaUpgrades[currentUpgrade + 1].cost))
            return;

        //Buy upgrade and scale up the area
        currentUpgrade += 1;
        ShopManager.Instance.BuyUnit(areaUpgrades[currentUpgrade].cost);
        UpdateAreaScale();

        if (currentUpgrade >= areaUpgrades.Count - 1)
            gameObject.SetActive(false);
        else
            UpdatePrice();
    }

    private void ResetArea()
    {
        gameObject.SetActive(true);

        //Reset upgrades and set the scale to the starting scale
        currentUpgrade = 0;
        UpdateAreaScale();
        UpdatePrice();
    }

    //Scale up placing area and simulate physics to update area box collider
    private void UpdateAreaScale()
    {
        areaTransform.localScale = maxScale * areaUpgrades[currentUpgrade].multiplier;
        Physics.Simulate(Time.deltaTime);
    }

    private void UpdatePrice() => price.text = areaUpgrades[currentUpgrade + 1].cost.ToString();
    private void OnDestroy() => EventManager.OnGameRestarted -= ResetArea;
}

[System.Serializable]
public class AreaUpgrade
{
    public float multiplier;
    public int cost;
}
