using UnityEngine;

public class CameraControllerButton : MonoBehaviour
{
    [SerializeField] Camera targetCamera;

    public void ChangeCamera() => CameraManager.Instance.SetCurrentCamera(targetCamera);
}