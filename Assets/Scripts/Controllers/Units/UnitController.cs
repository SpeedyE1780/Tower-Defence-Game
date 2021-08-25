using System.Runtime.CompilerServices;
using UnityEngine;

[DisallowMultipleComponent, SelectionBase]
public abstract class UnitController : MonoBehaviour
{
    protected const string PlayerTag = "Player";
    protected const string IdleAnimation = "Idle";
    protected const string AttackAnimation = "Attack";
    protected const string RunAnimation = "Run";
    protected const string DeathAnimation = "Death";

    public static bool waitForWaveStart;

    [SerializeField] protected UnitID unitID;
    [SerializeField] private bool isEnemy;
    [SerializeField] protected Animation unitAnimation;
    [SerializeField] protected HealthController unitHealth;
    [Header("Attack Stats")]
    [SerializeField] protected float attackCooldown;

    protected float currentAttackCooldown;
    protected HealthController currentTarget;
    private int instanceID;

    protected virtual bool HasIdleUpdate => true;
    protected bool CanAttack => currentAttackCooldown < 0;

    protected virtual void OnEnable()
    {
        instanceID = GetInstanceID();

        if (unitAnimation != null)
            unitAnimation.Play(IdleAnimation);

        //Reset target and attack cooldown
        currentTarget = null;
        currentAttackCooldown = 0;

        UnitInfo unitInfo = new UnitInfo()
        {
            InstanceID = instanceID,
            Position = transform.position,
            UnitTypeID = unitID.TypeID,
            UnitMask = unitID.GetLayerMask()
        };

        UnitsManager.Instance.AddUnit(unitInfo, unitHealth);

        if (!isEnemy)
            UnitPlacementManager.RaiseUnitCount();
    }

    protected virtual void OnDisable()
    {
        UnitsManager.Instance.RemoveUnit(instanceID);

        if (!isEnemy)
            UnitPlacementManager.LowerUnitCount();
    }

    protected virtual void Update()
    {
        if (waitForWaveStart)
            return;

        if (IsTargetActive())
        {
            AttackTarget();
        }
        else
        {
            //Get new target
            currentTarget = UnitsManager.Instance.GetTarget(instanceID);

            if (HasIdleUpdate)
                Idle();
        }

        currentAttackCooldown -= Time.deltaTime;
    }

    private void LateUpdate() => UnitsManager.Instance.UpdateUnitPosition(instanceID, transform.position, unitHealth.Health);
    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(unitID, gameObject);
    protected virtual void ResetAttackCooldown() => currentAttackCooldown = attackCooldown;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual bool IsTargetActive() => currentTarget != null && !currentTarget.IsDead && currentTarget.gameObject.activeInHierarchy;
    protected abstract void AttackTarget();
    protected abstract void Idle();
}