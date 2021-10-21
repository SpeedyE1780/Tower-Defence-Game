using UnityEngine;

//Particle that spawn once unit is spawned
public class SpawnParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem spawnParticles;

    private void OnEnable()
    {
        spawnParticles.Play();
    }
}
