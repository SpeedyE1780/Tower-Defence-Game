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

    public void ShowMenuUI() => ToggleGameUI(MenuUI);
    public void ShowGameUI() => ToggleGameUI(GameUI);
    public void ShowEndGameUI() => ToggleGameUI(EndGameUI);

    //Enable the selected canvas and disable the other
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
    public void SetWaveDelay(int delay) => waveDelay.text = delay.ToString();

    public void ToggleUnitPlacementCanvas(bool toggle)
    {
        unitPlacement.enabled = toggle;
        waveDelay.gameObject.SetActive(toggle);
    }

    private IEnumerator ShowAnimatedText(GameObject waveText)
    {
        waveText.SetActive(true);

        //Play scale up animation
        yield return StartCoroutine(PlayTextScaleAnimation(waveText.transform, waveStartScale, waveEndScale));

        yield return new WaitForSeconds(waveTextScaleDuration * 0.5f);

        //Play scale down animation
        yield return StartCoroutine(PlayTextScaleAnimation(waveText.transform, waveEndScale, waveStartScale));

        waveText.SetActive(false);
    }

    private IEnumerator PlayTextScaleAnimation(Transform waveTransform, Vector3 startScale, Vector3 endScale)
    {
        float time = 0;

        while (time < waveTextScaleDuration)
        {
            waveTransform.localScale = Vector3.Lerp(startScale, endScale, time / waveTextScaleDuration);
            yield return null;
            time += Time.deltaTime;
        }
    }

    public void ShowWaveNumber(int number)
    {
        waveStarted.text = $"Wave {number}";
        StartCoroutine(ShowAnimatedText(waveStarted.gameObject));
    }

    public Coroutine ShowWaveCompleted() => StartCoroutine(ShowAnimatedText(waveCompleted));
    public Coroutine ShowPlaceUnits() => StartCoroutine(ShowAnimatedText(placeUnits.gameObject));
}