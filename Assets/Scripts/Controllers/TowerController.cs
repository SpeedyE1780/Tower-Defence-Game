using UnityEngine;

public class TowerController : RangedController
{
    [SerializeField] private Transform turret;
    protected override Transform RotationTransform => turret;
}