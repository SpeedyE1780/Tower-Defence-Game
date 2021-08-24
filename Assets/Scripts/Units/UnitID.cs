using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/Unit ID")]
public class UnitID : PoolID
{
    [SerializeField] UnitType UnitType;
    [SerializeField] List<UnitType> targetTypes;

    public int TypeID => UnitType.Type;

    public int GetLayerMask()
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

    public string GetLayerMaskString()
    {
        return System.Convert.ToString(GetLayerMask(), 2);
    }

    #region EDITOR
#if UNITY_EDITOR
    [SerializeField] UnitType canAttack;

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
        Debug.Log("String method");
        string binary = System.Convert.ToString(id, 2).PadLeft(2, '0');
        Debug.Log($"Can Attack {canAttack.name}:{binary[binary.Length - 1 - canAttack.ID]}");

        Debug.Log("Binary method");
        int attackType = canAttack.Type;
        Debug.Log(attackType & id);
    }
#endif
    #endregion
}