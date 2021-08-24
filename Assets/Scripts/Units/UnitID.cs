using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/Unit ID")]
public class UnitID : PoolID
{
    [SerializeField] UnitType UnitType;
    [SerializeField] List<UnitType> targetTypes;

    #region
#if UNITY_EDITOR
    [SerializeField] UnitType canAttack;

    private int GetLayerMask()
    {
        int id = 0;

        foreach (UnitType type in targetTypes)
        {
            if (type == null)
                continue;

            id |= type.Type;
        }

        return id;
    }

    [ContextMenu("Check")]
    public void Check()
    {
        ShowIDMask();
        CanAttack();
    }

    private void ShowIDMask()
    {
        int id = GetLayerMask();
        Debug.Log($"{id}");
        Debug.Log($"Layer Mask: {System.Convert.ToString(id, 2).PadLeft(2, '0')}");
    }

    private void CanAttack()
    {
        if (canAttack == null)
            return;

        int id = GetLayerMask();
        string binary = System.Convert.ToString(id, 2).PadLeft(2, '0');
        Debug.Log($"Can Attack {canAttack.name}:{binary[binary.Length - 1 - canAttack.ID]}");
    }
#endif
    #endregion
}