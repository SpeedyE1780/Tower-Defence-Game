using UnityEngine;

public abstract class PlacementManager : MonoBehaviour
{
    public static bool IsPlacingUnits { get; protected set; }
    public static bool CanPlaceUnits { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void SetIsPlacingUnits() => IsPlacingUnits = false;
    public static void SetCanPlaceUnits(bool state) => CanPlaceUnits = state;
}