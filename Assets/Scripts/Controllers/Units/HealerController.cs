using UnityEngine;

public class HealerController : RangedController
{
    [SerializeField] private int healAmount;

    protected override void Shoot()
    {
        if (!CanAttack)
            return;

        ResetAttackCooldown();
        bulletCasing.Play();

        //Heal target & set to null to find a new target the next frame
        currentTarget.Heal(healAmount);
        currentTarget = null;
    }
}