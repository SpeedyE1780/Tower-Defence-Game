using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class HealerController : RangedController
{
    [SerializeField] protected int healAmount;
    [SerializeField] protected int healMana;
    [SerializeField] private Slider manaBar;
    private int usedMana;

    private bool CanHeal => usedMana <= healMana;

    protected override void OnEnable()
    {
        base.OnEnable();
        manaBar.gameObject.SetActive(true);
        EventManager.OnWaveEnded += ResetCharges;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        manaBar.gameObject.SetActive(false);
        EventManager.OnWaveEnded -= ResetCharges;
    }

    protected override void Shoot()
    {
        if (!CanAttack())
            return;

        //Heal target & set to null to find a new target the next frame
        Heal();
        currentTarget = null;

        UpdateManaBar();
        ResetAttackCooldown();
    }

    protected override bool CanAttack() => base.CanAttack() && CanHeal;

    private void UpdateManaBar()
    {
        usedMana++;
        manaBar.value = 1 - (float)usedMana / healMana;
    }

    private void ResetCharges()
    {
        usedMana = 0;
        manaBar.value = 1;
        UpdateManaBar();
    }

    protected virtual void Heal() => currentTarget.Heal(healAmount);
}
