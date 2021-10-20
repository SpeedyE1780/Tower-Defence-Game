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

    private bool AttackCooldownEnded => currentAttackCooldown <= 0;

    #region UNITY MESSAGES

    protected virtual void Awake() => unitMask = unitID.GetLayerMask();

    protected virtual void OnEnable()
    {
        instanceID = GetInstanceID();

        if (unitAnimation != null)
            unitAnimation.Play(IdleAnimation);

        //Reset target and attack cooldown
        currentTarget = null;
        currentAttackCooldown = attackCooldown;

        UnitInfo unitInfo = new UnitInfo()
        {
            instanceID = instanceID,
            position = transform.position,
            unitTypeID = unitID.TypeID,
            unitMask = unitMask
        };

        AddUnitToSet(unitInfo);
    }

    protected virtual void OnDisable() => RemoveUnitFromSet();

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

    private void LateUpdate() => UpdateUnitInSet();

    #endregion

    #region SET FUNCTIONS

    private void AddUnitToSet(UnitInfo unitInfo)
    {
        UnitsManager.Instance.AddUnit(unitInfo, unitHealth);
        detectionSet.Add(unitInfo);
    }

    private void UpdateUnitInSet()
    {
        Vector3 position = transform.position;
        float healthPercentage = unitHealth.HealthPercentage;

        UnitsManager.Instance.UpdateUnitPosition(instanceID, position, healthPercentage);
        detectionSet.UpdateUnitInfo(instanceID, position, healthPercentage);
    }

    private void RemoveUnitFromSet()
    {
        UnitsManager.Instance.RemoveUnit(instanceID);
        detectionSet.Remove(instanceID);
    }

    #endregion

    #region INHERITED VIRTUAL FUNCTIONS

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(unitID, gameObject);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void ResetAttackCooldown() => currentAttackCooldown = attackCooldown;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual bool IsTargetActive() => currentTarget != null && !currentTarget.IsDead && currentTarget.gameObject.activeInHierarchy;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract void AttackTarget();
    protected virtual bool CanAttack() => AttackCooldownEnded;

    #endregion
}
