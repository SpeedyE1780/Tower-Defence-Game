namespace AudioManager
{
    public class MusicSourceController : AudioSourceController
    {
        public override void Subscribe()
        {
            AudioManager.OnToggleMusic += ToggleAudioSource;
        }

        public override void Unsubscribe()
        {
            AudioManager.OnToggleMusic -= ToggleAudioSource;
        }
    } 
}