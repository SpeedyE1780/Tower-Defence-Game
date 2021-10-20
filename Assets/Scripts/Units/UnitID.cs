using System.Collections.Generic;
using UnityEngine;

//Advanced Pool ID for units
[CreateAssetMenu(menuName = "Units/Unit ID")]
public class UnitID : PoolID
{
    [SerializeField] UnitType unitType;
    [SerializeField] List<UnitType> targetTypes; //Units types current unit can attack

    public int TypeID => unitType.Type;

    public int GetLayerMask()
    {
        int mask = 0; //Binary representation is 0000

        //Grounded representation 0001
        //Airborne representation 0010
        //Enemy grounded representation 0100
        //Enemy airborne representation 1000
        //Every type in target types will flip the bit in id's binary representation
        //Unit A that can attack both enemy type will result in the mask's binary representation being 1100
        //To check wether or not a unit can be targeted or no we check the mask's binary representation and the type binary representation > 0
        //Grounded & Unit A => 0001 & 1100 => 0000 == 0 can't target
        //Enemy grounded & Unit A => 0100 & 1100 => 0100 = 4 > 0 can be targeted

        foreach (UnitType type in targetTypes)
            mask |= type.Type;

        return mask;
    }

    //Editor functions to show mask and check if unit can attack type assigned to canAttack
    #region EDITOR
#if UNITY_EDITOR

    [SerializeField] UnitType canAttack;

    [ContextMenu("Can Attack")]
    public void CheckCanAttack()
    {
        ShowIDMask();
        CanAttack();
    }

    private void ShowIDMask() => Debug.Log(GetLayerMask());

    private void CanAttack()
    {
        if (canAttack == null)
        {
            Debug.LogError("Can attack is null");
            return;
        }

        int id = GetLayerMask();
        int attackType = canAttack.Type;
        Debug.Log((attackType & id) != 0);
    }

#endif
    #endregion
}
