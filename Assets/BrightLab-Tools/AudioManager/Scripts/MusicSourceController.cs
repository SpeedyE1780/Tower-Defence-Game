namespace AudioManager
{
    public class MusicSourceController : AudioSourceController
    {
        protected override void SetAudioSourceState()
        {
            enabled = !AudioManager.Instance.IsMasterMuted && !AudioManager.Instance.IsMusicMuted;
        }

        protected override void Subscribe()
        {
            AudioManager.OnToggleMusic += ToggleAudioSource;
        }

        protected override void Unsubscribe()
        {
            AudioManager.OnToggleMusic -= ToggleAudioSource;
        }
    }
}