using UnityEngine;

namespace AudioManager
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AudioSourceController : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        private void Awake()
        {
            Subscribe();
            SetAudioSourceState();
        }

        private void OnDestroy() => Unsubscribe();
        protected abstract void SetAudioSourceState();
        protected void ToggleAudioSource(bool toggle) => source.enabled = toggle;
    }
}