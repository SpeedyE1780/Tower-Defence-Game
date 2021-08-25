using UnityEngine;

public class HealerController : RangedController
{
    [SerializeField] private int healAmount;

    protected override void Shoot()
    {
        if (!CanAttack)
            return;

        ResetAttackCooldown();
        currentTarget.Heal(healAmount);
        bulletCasing.Play();
    }
}