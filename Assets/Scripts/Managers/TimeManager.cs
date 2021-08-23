using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float timeScale;
    [SerializeField] private int targetFrameRate;

    private void Awake() => Application.targetFrameRate = targetFrameRate;

    public void PauseGame()
    {
        UIManager.Instance.TogglePauseUI(true);
        UpdateTimeScale(0);
    }

    public void ResumeGame()
    {
        UIManager.Instance.TogglePauseUI(false);
        UpdateTimeScale(1);
    }

    private void UpdateTimeScale(float scale)
    {
        timeScale = scale;
        Time.timeScale = timeScale;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            Time.timeScale = timeScale;
        }
    }
#endif
}