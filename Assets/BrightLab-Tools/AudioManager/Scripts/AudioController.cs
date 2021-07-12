using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AudioManager
{
    public class AudioController : MonoBehaviour
    {
        #region PLAYER PREFS KEYS

        private const string MusicMutedKey = "MusicMuted";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SFXMutedKey = "SFXMuted";
        private const string SFXVolumeKey = "SFXVolume";

        #endregion

        [Header("Sound Control")]
        public Slider MusicVolume;
        public Toggle MuteMusic;
        public Slider SFXVolume;
        public Toggle MuteSFX;

        [Header("Toggle Icons")]
        public Sprite UnMuted;
        public Sprite Muted;

        private Dictionary<bool, Sprite> toggleIcon;
        private Image MusicToggleImage;
        private Image SFXToggleImage;

        #region  INITIALIZE

        private void Awake()
        {
            toggleIcon = new Dictionary<bool, Sprite>
            {
                { true, Muted },
                { false, UnMuted }
            };

            GetToggleImages();
            SetDefault();
            UpdateAudioManager();
            AddListeners();
            UpdateToggleImages();
        }

        private void GetToggleImages()
        {
            MusicToggleImage = MuteMusic.GetComponent<Image>();
            SFXToggleImage = MuteSFX.GetComponent<Image>();
        }

        private void SetDefault()
        {
            MuteMusic.isOn = PlayerPrefs.GetInt(MusicMutedKey, 0) == 1;
            MusicVolume.value = PlayerPrefs.GetFloat(MusicVolumeKey, 1);
            MuteSFX.isOn = PlayerPrefs.GetInt(SFXMutedKey, 0) == 1;
            SFXVolume.value = PlayerPrefs.GetFloat(SFXVolumeKey, 1);
        }

        private void UpdateAudioManager()
        {
            AudioManager.Instance.SetMusicVolume(MusicVolume.value);
            AudioManager.Instance.SetSFXVolume(SFXVolume.value);
            AudioManager.Instance.ToggleMusic(MuteMusic.isOn);
            AudioManager.Instance.ToggleSFX(MuteSFX.isOn);
        }

        private void AddListeners()
        {
            MusicVolume.onValueChanged.AddListener(ControlMusic);
            SFXVolume.onValueChanged.AddListener(ControlSFX);
            MuteMusic.onValueChanged.AddListener(ToggleMusic);
            MuteSFX.onValueChanged.AddListener(ToggleSFX);
        }

        private void UpdateToggleImages()
        {
            MusicToggleImage.sprite = toggleIcon[MuteMusic.isOn];
            SFXToggleImage.sprite = toggleIcon[MuteSFX.isOn];
        }

        #endregion

        #region FUNCTIONS

        public void ControlMusic(float musicVolume)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
            AudioManager.Instance.SetMusicVolume(musicVolume);

            bool musicMuted = musicVolume == 0;
            if (musicMuted != MuteMusic.isOn)
                ToggleMusic(musicMuted);
        }

        public void ControlSFX(float sfxVolume)
        {
            PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolume);
            AudioManager.Instance.SetSFXVolume(sfxVolume);

            bool sfxMuted = sfxVolume == 0;
            if (sfxMuted != MuteSFX.isOn)
                ToggleSFX(sfxMuted);
        }

        public void ToggleMusic(bool musicMuted)
        {
            int value = musicMuted ? 1 : 0;
            PlayerPrefs.SetInt(MusicMutedKey, value);
            AudioManager.Instance.ToggleMusic(musicMuted);
            MusicToggleImage.sprite = toggleIcon[musicMuted];
        }

        public void ToggleSFX(bool sfxMuted)
        {
            int value = sfxMuted ? 1 : 0;
            PlayerPrefs.SetInt(SFXMutedKey, value);
            AudioManager.Instance.ToggleSFX(sfxMuted);
            SFXToggleImage.sprite = toggleIcon[sfxMuted];
        }

        #endregion
    }
}