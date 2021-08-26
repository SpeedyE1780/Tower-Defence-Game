using UnityEngine;

public class HealerController : RangedController
{
    [SerializeField] protected int healAmount;

    protected override void Shoot()
    {
        if (!CanAttack)
            return;

        //Heal target & set to null to find a new target the next frame
        Heal();
        currentTarget = null;

        ResetAttackCooldown();
    }

    protected virtual void Heal() => currentTarget.Heal(healAmount);
}