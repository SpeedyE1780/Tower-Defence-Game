using UnityEngine;

public abstract class PlacementManager : MonoBehaviour
{
    public static bool CanPlaceUnits { get; protected set; }

    [RuntimeInitializeOnLoadMethod]
    private static void SetCanPlaceUnits() => CanPlaceUnits = true;
}