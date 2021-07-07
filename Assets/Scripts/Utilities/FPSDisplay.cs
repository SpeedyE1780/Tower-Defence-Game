using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private int targetRate;
    [SerializeField] private Text fpsText;
    float deltaTime = 0.0f;
    string text;
    float fps;
    float msec;


    private void Awake()
    {
        Application.targetFrameRate = targetRate;
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
        text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        fpsText.text = text;
    }
}