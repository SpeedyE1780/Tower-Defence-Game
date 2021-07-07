using UnityEngine;

public class TowerShootingController : UnitShootingController
{
    [SerializeField] private Transform turret;
    protected override Transform RotationTransform => turret;
}