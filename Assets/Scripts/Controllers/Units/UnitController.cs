using System.Collections.Generic;
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

    [SerializeField] protected PoolID poolID;
    [SerializeField] private bool isEnemy;
    [SerializeField] protected Animation unitAnimation;
    [Header("Attack Stats")]
    [SerializeField] protected float attackCooldown;

    protected float currentAttackCooldown;
    [SerializeField] protected HealthController currentTarget;

    protected virtual bool HasIdleUpdate => true;

    protected virtual void OnEnable()
    {
        unitAnimation.Play(IdleAnimation);
        currentTarget = null;
        currentAttackCooldown = attackCooldown;
        UnitsManager.AddUnit(isEnemy, transform);
    }

    protected virtual void OnDisable()
    {
        UnitsManager.RemoveUnit(isEnemy, transform);
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
            Transform target = UnitsManager.GetTarget(!isEnemy);
            if (target != null)
                currentTarget = target.GetComponent<HealthController>();
            else
                currentTarget = null;

            if (HasIdleUpdate)
                Idle();
        }

        currentAttackCooldown -= Time.deltaTime;
    }

    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(poolID, gameObject);
    protected virtual void ResetCooldown() => currentAttackCooldown = attackCooldown;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual bool IsTargetActive() => currentTarget != null && !currentTarget.IsDead && currentTarget.gameObject.activeInHierarchy;
    protected abstract void AttackTarget();
    protected abstract void Idle();
}