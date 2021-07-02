using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementManager : Singleton<UnitPlacementManager>
{
    [SerializeField] private TowerPlacementController towerPlacement;
    [SerializeField] private UnitPlacementController unitPlacement;
    [SerializeField] private GameObject unit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PlaceUnit(unit);
    }

    public void PlaceTower(TowerShootingController tower)
    {
        towerPlacement.StartTowerPlacement(tower);
    }

    public void PlaceUnit(GameObject unit)
    {
        unitPlacement.StartUnitPlacement(unit);
    }
}