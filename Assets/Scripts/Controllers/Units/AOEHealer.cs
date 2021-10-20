using UnityEngine;

public class AOEHealer : HealerController
{
    [SerializeField] float range;
    [SerializeField] ParticleSystem healingParticle;

    protected override void Awake()
    {
        base.Awake();
        healingParticle.transform.localScale = Vector3.one * range;
    }

    protected override void Heal()
    {
        Vector3 position = currentTarget.transform.position;
        AOEManager.Instance.ApplyAOEHeal(position, range, healAmount, unitMask);
        PlayParticleSystem(position);
    }

    private void PlayParticleSystem(Vector3 position)
    {
        healingParticle.transform.position = position;
        healingParticle.Play();
    }
}
