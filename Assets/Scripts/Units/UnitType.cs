using UnityEngine;

[CreateAssetMenu(menuName = "Units/Unit Type")]
public class UnitType : ScriptableObject
{
    [SerializeField] private int typeID; //Unit ID needs to be different for each type
    [SerializeField] private int typeNumber; //Type number is (2 ^ typeID) so that its binary representation only has 1 at the index of typeID

    public int ID => typeID;
    public int Type => typeNumber;

    private void OnValidate()
    {
        typeNumber = (int)Mathf.Pow(2, typeID);
    }
}
