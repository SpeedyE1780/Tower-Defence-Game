using UnityEngine;

public abstract class PlacementManager : MonoBehaviour
{
    public static bool IsPlacingUnits { get; protected set; }

    [RuntimeInitializeOnLoadMethod]
    private static void SetIsPlacingUnits() => IsPlacingUnits = false;
}