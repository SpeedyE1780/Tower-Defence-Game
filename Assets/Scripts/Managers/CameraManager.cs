using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera defaultCamera;
    [SerializeField] private List<Camera> gameCameras;

    private void Start() => SetDefaultCamera();
    public void SetDefaultCamera() => SetCurrentCamera(defaultCamera);

    //Disable all cameras except target camera
    private void ToggleCameras(Camera targetCamera) => gameCameras.ForEach(camera => camera.gameObject.SetActive(camera == targetCamera));

    public void SetCurrentCamera(Camera camera)
    {
        ToggleCameras(camera);
        CameraRay.SetCurrentCamera(camera);
    }
}