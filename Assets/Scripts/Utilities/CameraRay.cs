using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraRay
{
    public static Camera MainCamera { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void SetMainCamera()
    {
        MainCamera = Camera.main;
    }

    public static Ray GetCameraRay()
    {
        return MainCamera.ScreenPointToRay(Input.mousePosition);
    }

    public static bool GetCameraHitPoint(out Vector3 hitPoint, LayerMask layerMask)
    {
        Ray ray = GetCameraRay();
        bool successfulHit = Physics.Raycast(ray, out RaycastHit hit, MainCamera.farClipPlane, layerMask);
        hitPoint = hit.point;
        return successfulHit;
    }
}