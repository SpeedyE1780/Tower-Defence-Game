using Unity.Mathematics;

public struct UnitInfo
{
    public int instanceID;
    public float3 position;
    public float healthPercentage;
    public int targetID;
    public int unitTypeID;
    public int unitMask;

    public bool CanTarget(int otherInstanceID, int otherTypeID) => IsDifferentUnit(otherInstanceID) && IsValidTarget(otherTypeID);
    private bool IsDifferentUnit(int otherInstanceID) => instanceID != otherInstanceID;
    private bool IsValidTarget(int otherTypeID) => (unitMask & otherTypeID) != 0;
}
