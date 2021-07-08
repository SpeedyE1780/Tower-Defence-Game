using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Game Menues")]
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject PauseUI;

    [Header("Game UI")]
    [SerializeField] private Text coinText;
    [SerializeField] private Text killsText;
    [SerializeField] private Text waveStarted;
    [SerializeField] private GameObject waveCompleted;
    [SerializeField] private AnimationClip waveTextAnimation;

    private void Start()
    {
        ShowMenuUI();
    }

    public void ShowMenuUI()
    {
        GameUI.SetActive(false);
        PauseUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    public void ShowGameUI()
    {
        MenuUI.SetActive(false);
        PauseUI.SetActive(false);
        GameUI.SetActive(true);
    }

    public void TogglePauseUI(bool state) => PauseUI.SetActive(state);


    public void UpdateKillText(int score) => killsText.text = $"Kills: {score}";
    public void UpdateCurrencyText(int coins) => coinText.text = coins.ToString();

    public IEnumerator ShowWaveCompleted()
    {
        waveCompleted.SetActive(true);
        yield return new WaitForSeconds(waveTextAnimation.length);
        waveCompleted.SetActive(false);
    }

    public IEnumerator ShowWaveNumber(int number)
    {
        waveStarted.gameObject.SetActive(true);
        waveStarted.text = $"Wave {number}";
        yield return new WaitForSeconds(waveTextAnimation.length);
        waveStarted.gameObject.SetActive(false);
    }
}