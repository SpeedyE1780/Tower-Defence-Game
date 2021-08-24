using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/Unit ID")]
public class UnitID : PoolID
{
    [SerializeField] UnitType UnitType;
    [SerializeField] List<UnitType> targetTypes;

    private void OnValidate()
    {
        ShowIDMask();
    }

    private void ShowIDMask()
    {
        int id = 0;

        foreach (UnitType type in targetTypes)
        {
            id |= type.Type;
        }

        Debug.Log(id);
        Debug.Log(System.Convert.ToString(id, 2));
    }
}