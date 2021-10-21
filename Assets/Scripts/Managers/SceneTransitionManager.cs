using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    [SerializeField] CanvasGroup group;
    [SerializeField] Canvas canvas;

    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        base.Awake();
        DontDestroyOnLoad(gameObject);
        canvas.enabled = false;
    }

    public void StartSceneLoading(int sceneIndex)
    {
        canvas.enabled = true;
        StartCoroutine(LoadScene(sceneIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        yield return StartCoroutine(FadeScene(0, 1));
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return new WaitUntil(() => loadOperation.isDone);
        yield return StartCoroutine(FadeScene(1, 0));
        canvas.enabled = false;
    }

    IEnumerator FadeScene(float start, float end)
    {
        float timer = 0;
        float duration = 0.5f;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, timer / duration);
        }
    }
}
