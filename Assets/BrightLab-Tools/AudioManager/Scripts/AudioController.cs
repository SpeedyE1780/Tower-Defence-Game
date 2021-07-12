using UnityEngine;
using UnityEngine.UI;

namespace AudioManager
{
    public class AudioController : MonoBehaviour
    {
        [Header("Master")]
        [SerializeField] private Slider MasterVolume;
        [SerializeField] private Toggle MuteMaster;
        [Header("Music")]
        [SerializeField] private Slider MusicVolume;
        [SerializeField] private Toggle MuteMusic;
        [Header("SFX")]
        [SerializeField] private Slider SFXVolume;
        [SerializeField] private Toggle MuteSFX;

        private void Start()
        {
            SetDefault();
            AddListeners();
        }

        private void SetDefault()
        {
            MasterVolume.value = AudioManager.Instance.MasterVolume;
            MuteMaster.isOn = AudioManager.Instance.IsMasterMuted;
            MusicVolume.value = AudioManager.Instance.MusicVolume;
            MuteMusic.isOn = AudioManager.Instance.IsMusicMuted;
            SFXVolume.value = AudioManager.Instance.SFXVolume;
            MuteSFX.isOn = AudioManager.Instance.IsSFXMuted;
        }

        private void AddListeners()
        {
            MasterVolume.onValueChanged.AddListener(AudioManager.Instance.UpdateMasterVolume);
            MusicVolume.onValueChanged.AddListener(AudioManager.Instance.UpdateMusicVolume);
            SFXVolume.onValueChanged.AddListener(AudioManager.Instance.UpdateSFXVolume);
            MuteMaster.onValueChanged.AddListener(AudioManager.Instance.ToggleMaster);
            MuteMusic.onValueChanged.AddListener(AudioManager.Instance.ToggleMusic);
            MuteSFX.onValueChanged.AddListener(AudioManager.Instance.ToggleSFX);
        }
    }
}