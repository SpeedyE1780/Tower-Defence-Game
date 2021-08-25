using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int scene) => SceneTransitionManager.Instance.StartSceneLoading(scene);
}