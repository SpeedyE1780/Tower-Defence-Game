using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float timeScale;

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            Time.timeScale = timeScale;
        }
    }
}