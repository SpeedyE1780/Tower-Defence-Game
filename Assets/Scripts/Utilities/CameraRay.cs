using UnityEngine;

public static class CameraRay
{
    public static Camera MainCamera { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void SetMainCamera() => MainCamera = Camera.main;

    private static bool ShootRaycast(out RaycastHit hit, LayerMask layerMask)
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition); ;
        return Physics.Raycast(ray, out hit, MainCamera.farClipPlane, layerMask);
    }

    //Get unit hit by raycast
    public static bool GetCameraHitUnit(out Transform unit, LayerMask layerMask)
    {
        bool successfulHit = ShootRaycast(out RaycastHit hit, layerMask);
        unit = hit.transform;
        return successfulHit;
    }

    //Get position hit by raycast
    public static bool GetCameraHitPoint(out Vector3 hitPoint, LayerMask layerMask)
    {
        bool successfulHit = ShootRaycast(out RaycastHit hit, layerMask);
        hitPoint = hit.point;
        return successfulHit;
    }
}