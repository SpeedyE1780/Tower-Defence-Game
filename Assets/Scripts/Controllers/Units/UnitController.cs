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

    protected static Dictionary<bool, List<Transform>> activeUnits;
    public static bool waitForWaveStart;

    [SerializeField] protected PoolID poolID;
    [SerializeField] private bool isEnemy;
    [SerializeField] protected Animation unitAnimation;
    [Header("Attack Stats")]
    [SerializeField] protected float attackCooldown;

    protected float currentAttackCooldown;
    [SerializeField] protected HealthController currentTarget;

    protected virtual bool HasIdleUpdate => true;

    [RuntimeInitializeOnLoadMethod]
    private static void InitializeUnit()
    {
        activeUnits = new Dictionary<bool, List<Transform>>();
        activeUnits[true] = new List<Transform>();
        activeUnits[false] = new List<Transform>();
    }

    protected virtual void OnEnable()
    {
        unitAnimation.Play(IdleAnimation);
        currentTarget = null;
        currentAttackCooldown = attackCooldown;
        activeUnits[isEnemy].Add(transform);
    }

    protected virtual void OnDisable()
    {
        activeUnits[isEnemy].Remove(transform);
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
            if (activeUnits[!isEnemy].Count > 0)
                currentTarget = activeUnits[!isEnemy].GetRandomElement().GetComponent<HealthController>();
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