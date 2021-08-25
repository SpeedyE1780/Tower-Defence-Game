using UnityEngine;

public class HealerController : RangedController
{
    [SerializeField] protected int healAmount;

    protected override void Shoot()
    {
        if (!CanAttack)
            return;

        ResetAttackCooldown();
        Heal();
    }

    protected virtual void Heal()
    {
        //Heal target & set to null to find a new target the next frame
        currentTarget.Heal(healAmount);
        currentTarget = null;
    }
}