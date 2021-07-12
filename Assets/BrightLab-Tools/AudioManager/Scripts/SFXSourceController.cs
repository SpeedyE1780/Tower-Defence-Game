namespace AudioManager
{
    public class SFXSourceController : AudioSourceController
    {
        protected override void SetAudioSourceState()
        {
            enabled = !AudioManager.Instance.IsMasterMuted && !AudioManager.Instance.IsSFXMuted;
        }

        protected override void Subscribe()
        {
            AudioManager.OnToggleSFX += ToggleAudioSource;
        }

        protected override void Unsubscribe()
        {
            AudioManager.OnToggleSFX -= ToggleAudioSource;
        }
    }
}