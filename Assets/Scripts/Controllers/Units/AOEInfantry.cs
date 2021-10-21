using UnityEngine;

public class AOEInfantry : InfantryController
{
    [SerializeField] private float range;

    protected override void ApplyDamage() => AOEManager.Instance.ApplyAOEDamage(currentTarget.transform.position, range, damage, unitMask);
}
