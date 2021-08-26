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

    [SerializeField] private UnitID unitID;
    [SerializeField] protected Animation unitAnimation;
    [SerializeField] protected HealthController unitHealth;
    [SerializeField] protected UnitsInfoSet detectionSet;
    [Header("Attack Stats")]
    [SerializeField] protected float attackCooldown;

    protected float currentAttackCooldown;
    protected HealthController currentTarget;
    protected int unitMask;
    private int instanceID;

    protected bool CanAttack => currentAttackCooldown < 0;

    private void Awake() => unitMask = unitID.GetLayerMask();

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
        detectionSet.Add(unitInfo);
    }

    protected virtual void OnDisable()
    {
        UnitsManager.Instance.RemoveUnit(instanceID);
        detectionSet.Remove(instanceID);
    }

    protected virtual void Update()
    {
        if (waitForWaveStart)
            return;

        //Attack current target or get a new one
        if (IsTargetActive())
            AttackTarget();
        else
            currentTarget = UnitsManager.Instance.GetTarget(instanceID);

        currentAttackCooldown -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        float healthPercentage = unitHealth.HealthPercentage;

        UnitsManager.Instance.UpdateUnitPosition(instanceID, position, healthPercentage);
        detectionSet.UpdateUnitInfo(instanceID, position, healthPercentage);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(unitID, gameObject);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void ResetAttackCooldown() => currentAttackCooldown = attackCooldown;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual bool IsTargetActive() => currentTarget != null && !currentTarget.IsDead && currentTarget.gameObject.activeInHierarchy;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract void AttackTarget();
}