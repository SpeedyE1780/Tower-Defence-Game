using UnityEngine;

public static class CameraRay
{
    private static Camera currentCamera;

    public static void SetCurrentCamera(Camera current) => currentCamera = current;

    private static bool ShootRaycast(out RaycastHit hit, LayerMask layerMask)
    {
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition); ;
        return Physics.Raycast(ray, out hit, currentCamera.farClipPlane, layerMask);
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