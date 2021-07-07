using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text killsText;
    [SerializeField] private Text waveStarted;
    [SerializeField] private GameObject waveCompleted;
    [SerializeField] private AnimationClip waveTextAnimation;

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