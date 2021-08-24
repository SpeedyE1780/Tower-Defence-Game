using UnityEngine;

[CreateAssetMenu(menuName = "Units/Unit Type")]
public class UnitType : ScriptableObject
{
    [SerializeField] private int typeID;
    [SerializeField] private int typeNumber;

    public int Type => typeNumber;

    private void OnValidate()
    {
        typeNumber = (int)Mathf.Pow(2, typeID);
    }
}