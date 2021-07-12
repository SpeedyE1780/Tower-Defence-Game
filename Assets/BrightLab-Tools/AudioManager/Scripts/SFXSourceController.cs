namespace AudioManager
{
    public class SFXSourceController : AudioSourceController
    {
        public override void Subscribe()
        {
            AudioManager.OnToggleSFX += ToggleAudioSource;
        }

        public override void Unsubscribe()
        {
            AudioManager.OnToggleSFX -= ToggleAudioSource;
        }
    }
}