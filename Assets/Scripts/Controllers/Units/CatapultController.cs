using UnityEngine;

public class CatapultController : RangedController
{
    [SerializeField] private Transform catapult;
    protected override Transform RotationTransform => catapult;
}