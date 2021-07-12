using UnityEngine;

namespace AudioManager
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AudioSourceController : MonoBehaviour
    {
        private AudioSource source;
        public AudioSource AudioSource
        {
            get
            {
                if (source == null)
                    source = GetComponent<AudioSource>();

                return source;
            }
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();
        private void OnDestroy() => Unsubscribe();
        protected void ToggleAudioSource(bool toggle) => AudioSource.enabled = toggle;
    }
}