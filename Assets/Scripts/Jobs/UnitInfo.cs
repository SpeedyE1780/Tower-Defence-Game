using Unity.Mathematics;

public struct UnitInfo
{
    public int InstanceID;
    public float3 Position;
    public float HealthPercentage;
    public int TargetID;
    public int UnitTypeID;
    public int UnitMask;

    public bool CanAttack(int otherInstanceID, int otherTypeID) => IsDifferentUnit(otherInstanceID) && IsValidTarget(otherTypeID);
    private bool IsDifferentUnit(int otherInstanceID) => InstanceID != otherInstanceID;
    private bool IsValidTarget(int otherTypeID) => (UnitMask & otherTypeID) != 0;
}