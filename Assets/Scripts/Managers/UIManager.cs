using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private static Vector3 waveStartScale = new Vector3(0, 0, 0);
    private static Vector3 waveEndScale = new Vector3(1, 1, 1);

    [Header("Game Menues")]
    [SerializeField] private Canvas MenuUI;
    [SerializeField] private Canvas GameUI;
    [SerializeField] private Canvas PauseUI;
    [SerializeField] private Canvas EndGameUI;

    [Header("Game UI")]
    [SerializeField] private Text coinText;
    [SerializeField] private Text killsText;
    [SerializeField] private Text waveDelay;
    [SerializeField] private Text placeUnits;
    [SerializeField] private Text waveStarted;
    [SerializeField] private GameObject waveCompleted;
    [SerializeField] private float waveTextScaleDuration;
    [SerializeField] private Canvas unitPlacement;

    private void Start() => ShowMenuUI();

    public void ShowMenuUI()
    {
        ToggleGameUI(MenuUI);
    }

    public void ShowGameUI()
    {
        ToggleGameUI(GameUI);
    }

    public void ShowEndGameUI()
    {
        ToggleGameUI(EndGameUI);
    }

    private void ToggleGameUI(Canvas current)
    {
        MenuUI.enabled = current == MenuUI;
        GameUI.enabled = current == GameUI;
        PauseUI.enabled = current == PauseUI;
        EndGameUI.enabled = current == EndGameUI;
    }

    public void TogglePauseUI(bool state) => PauseUI.enabled = state;
    public void UpdateKillText(int score) => killsText.text = score.ToString();
    public void UpdateCurrencyText(int coins) => coinText.text = coins.ToString();
    public void ToggleUnitPlacementCanvas(bool toggle)
    {
        unitPlacement.enabled = toggle;
        waveDelay.gameObject.SetActive(toggle);
    }
    public void SetWaveDelay(int delay) => waveDelay.text = delay.ToString();

    private IEnumerator ShowWaveText(GameObject waveText)
    {
        waveText.SetActive(true);
        Transform waveTransform = waveText.transform;
        float time = 0;

        while (time < waveTextScaleDuration)
        {
            waveTransform.localScale = Vector3.Lerp(waveStartScale, waveEndScale, time / waveTextScaleDuration);
            yield return null;
            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(waveTextScaleDuration * 0.5f);

        time = 0;
        while (time < waveTextScaleDuration)
        {
            waveTransform.localScale = Vector3.Lerp(waveEndScale, waveStartScale, time / waveTextScaleDuration);
            yield return null;
            time += Time.deltaTime;
        }

        waveText.SetActive(false);
    }

    public void ShowWaveNumber(int number)
    {
        waveStarted.text = $"Wave {number}";
        StartCoroutine(ShowWaveText(waveStarted.gameObject));
    }

    public Coroutine ShowWaveCompleted() => StartCoroutine(ShowWaveText(waveCompleted));
    public Coroutine ShowPlaceUnits() => StartCoroutine(ShowWaveText(placeUnits.gameObject));
}