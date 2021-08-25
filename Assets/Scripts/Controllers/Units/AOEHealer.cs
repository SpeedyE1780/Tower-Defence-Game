using UnityEngine;

public class AOEHealer : HealerController
{
    [SerializeField] float range;
    [SerializeField] ParticleSystem healingParticle;

    private void Awake()
    {
        healingParticle.transform.localScale = Vector3.one * range;
    }

    protected override void Heal()
    {
        Vector3 position = currentTarget.transform.position;
        AOEManager.Instance.ApplyAOEHeal(position, range, healAmount, unitID.GetLayerMask());
        healingParticle.transform.position = position;
        healingParticle.Play();
    }
}
