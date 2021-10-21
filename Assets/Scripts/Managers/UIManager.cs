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

    #region MENU FUNCTIONS

    private void Start() => ShowMenuUI();
    public void ShowMenuUI() => ToggleGameUI(MenuUI);
    public void ShowGameUI() => ToggleGameUI(GameUI);
    public void ShowEndGameUI() => ToggleGameUI(EndGameUI);

    #endregion

    #region TOGGLE FUNCTIONS

    //Enable the selected canvas and disable the other
    private void ToggleGameUI(Canvas current)
    {
        MenuUI.enabled = current == MenuUI;
        GameUI.enabled = current == GameUI;
        PauseUI.enabled = current == PauseUI;
        EndGameUI.enabled = current == EndGameUI;
    }

    public void TogglePauseUI(bool state) => PauseUI.enabled = state;
    public void ToggleWaveDelayText(bool toggle) => waveDelay.gameObject.SetActive(toggle);
    public void ToggleUnitPlacementCanvas(bool toggle) => unitPlacement.enabled = toggle;

    #endregion

    #region TEXT FUNCTIONS

    public void UpdateKillText(int score) => killsText.text = score.ToString();
    public void UpdateCoinText(int coins) => coinText.text = coins.ToString();
    public void UpdateWaveDelay(int delay) => waveDelay.text = delay.ToString();

    #endregion

    #region TEXT ANIMATIONS

    public Coroutine ShowWaveCompleted() => StartCoroutine(ShowAnimatedText(waveCompleted));

    public Coroutine ShowWaveDelayUI(int number)
    {
        waveStarted.text = $"Wave {number}";
        StartCoroutine(ShowAnimatedText(waveStarted.gameObject));
        return StartCoroutine(ShowAnimatedText(placeUnits.gameObject));
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

    #endregion
}
