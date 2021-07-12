using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        #region AUDIO MIXER EXPOSED PARAMETERS

        private const string MixerMasterVolume = "MasterVolume";
        private const string MixerMusicVolume = "MusicVolume";
        private const string MixerSFXVolume = "SFXVolume";

        #endregion

        #region TOGGLE EVENTS

        public delegate void ToggleAudioSource(bool toggle);
        public static event ToggleAudioSource OnToggleMusic;
        public static event ToggleAudioSource OnToggleSFX;

        #endregion

        #region FIELDS

        public static AudioManager Instance { get; private set; }

        #region PRIVATE VARIABLES

        [Header("Audio Mixer Settings")]
        [SerializeField] private AudioMixer gameMixer;
        [SerializeField] private float minimumVolume;
        [SerializeField] private float maximumVolume;

        #endregion

        #region PROPERTIES

        public float MasterVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float SFXVolume { get; private set; }
        public bool IsMasterMuted { get; private set; }
        public bool IsMusicMuted { get; private set; }
        public bool IsSFXMuted { get; private set; }

        #endregion

        #endregion

        #region INITIALIZE

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }


            SetDefault();
        }

        private void SetDefault()
        {
            UpdateMasterVolume(PlayerPrefs.GetFloat(MusicKeys.MasterVolumeKey, 1));
            UpdateMusicVolume(PlayerPrefs.GetFloat(MusicKeys.MusicVolumeKey, 1));
            UpdateSFXVolume(PlayerPrefs.GetFloat(MusicKeys.SFXVolumeKey, 1));
            IsMusicMuted = PlayerPrefs.GetInt(MusicKeys.MusicMutedKey, 0) == 1;
            IsSFXMuted = PlayerPrefs.GetInt(MusicKeys.SFXMutedKey, 0) == 1;
        }

        #endregion

        #region UI CALLBACKS

        #region VOLUME SLIDERS

        public void UpdateMasterVolume(float value)
        {
            MasterVolume = value;
            SetMasterVolume();
        }

        public void UpdateMusicVolume(float value)
        {
            MusicVolume = value;
            SetMusicVolume();
        }

        public void UpdateSFXVolume(float value)
        {
            SFXVolume = value;
            SetSFXVolume();
        }

        #endregion

        #region TOGGLE CALLBACKS

        public void ToggleMaster(bool value)
        {
            IsMasterMuted = value;
            ToggleMusic();
            ToggleSFX();
        }

        public void ToggleMusic(bool value)
        {
            IsMusicMuted = value;
            ToggleMusic();
        }

        public void ToggleSFX(bool value)
        {
            IsSFXMuted = value;
            ToggleSFX();
        }

        #endregion

        #endregion

        #region UPDATE MIXER VOLUME

        private float GetVolume(float alpha) => Mathf.Lerp(minimumVolume, maximumVolume, alpha);
        public void SetMasterVolume() => gameMixer.SetFloat(MixerMasterVolume, GetVolume(MasterVolume));
        public void SetMusicVolume() => gameMixer.SetFloat(MixerMusicVolume, GetVolume(MusicVolume));
        public void SetSFXVolume() => gameMixer.SetFloat(MixerSFXVolume, GetVolume(SFXVolume));

        #endregion

        #region TOGGLE AUDIOSOURCES

        public void ToggleMusic() => OnToggleMusic?.Invoke(!IsMasterMuted && !IsMusicMuted);
        public void ToggleSFX() => OnToggleSFX?.Invoke(!IsMasterMuted && !IsSFXMuted);

        #endregion
    }
}