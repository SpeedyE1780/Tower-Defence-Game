using UnityEngine;

public static class CameraRay
{
    public static Camera MainCamera { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void SetMainCamera() => MainCamera = Camera.main;
    private static Ray GetCameraRay() => MainCamera.ScreenPointToRay(Input.mousePosition);

    public static bool GetCameraHitUnit(out Transform unit, LayerMask layerMask)
    {
        Ray ray = GetCameraRay();
        bool successfulHit = Physics.Raycast(ray, out RaycastHit hit, MainCamera.farClipPlane, layerMask);
        unit = hit.transform;
        return successfulHit;
    }

    public static bool GetCameraHitPoint(out Vector3 hitPoint, LayerMask layerMask)
    {
        Ray ray = GetCameraRay();
        bool successfulHit = Physics.Raycast(ray, out RaycastHit hit, MainCamera.farClipPlane, layerMask);
        hitPoint = hit.point;
        return successfulHit;
    }
}