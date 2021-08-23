using UnityEngine;

public class SpawnParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem spawnParticles;


    private void OnEnable()
    {
        spawnParticles.Play();
    }
}