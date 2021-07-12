using UnityEngine;
using UnityEngine.Audio;

namespace AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        #region AUDIO MIXER EXPOSED PARAMETERS

        private const string MasterVolume = "MasterVolume";
        private const string MusicVolume = "MusicVolume";
        private const string SFXVolume = "SFXVolume";

        #endregion

        #region TOGGLE EVENTS

        public delegate void ToggleAudioSource(bool toggle);
        public static event ToggleAudioSource OnToggleMusic;
        public static event ToggleAudioSource OnToggleSFX;

        #endregion

        public static AudioManager Instance { get; private set; }

        [Header("Audio Mixer Settings")]
        [SerializeField] private AudioMixer gameMixer;
        [SerializeField] private float minimumVolume;
        [SerializeField] private float maximumVolume;

        #region INITIALIZE

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }

        #endregion

        #region UPDATE MIXER VOLUME

        private float GetVolume(float alpha) => Mathf.Lerp(minimumVolume, maximumVolume, alpha);
        public void SetMasterVolume(float value) => gameMixer.SetFloat(MasterVolume, GetVolume(value));
        public void SetMusicVolume(float value) => gameMixer.SetFloat(MusicVolume, GetVolume(value));
        public void SetSFXVolume(float value) => gameMixer.SetFloat(SFXVolume, GetVolume(value));

        #endregion

        #region TOGGLE AUDIOSOURCES

        public void ToggleMusic(bool toggle) => OnToggleMusic?.Invoke(toggle);
        public void ToggleSFX(bool toggle) => OnToggleSFX?.Invoke(toggle);

        #endregion
    }
}