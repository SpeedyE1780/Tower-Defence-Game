using UnityEngine;

//Loads new scene on button click
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int scene) => SceneTransitionManager.Instance.StartSceneLoading(scene);
}
