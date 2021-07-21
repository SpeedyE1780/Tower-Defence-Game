using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class UnitController : MonoBehaviour
{
    protected const string IdleAnimation = "Idle";
    protected const string AttackAnimation = "Attack";
    protected const string RunAnimation = "Run";
    protected const string DeathAnimation = "Death";

    [SerializeField] protected PoolID poolID;
    [SerializeField] protected Animation unitAnimation;
    [Header("Detection Stats")]
    [SerializeField] private float detectionRaduis;
    [SerializeField] private LayerMask detectionLayer;
    [Header("Attack Stats")]
    [SerializeField] protected float attackCooldown;

    protected int commandIndex;
    protected float currentAttackCooldown;
    protected HealthController currentTarget;

    protected virtual bool HasIdleUpdate => true;

    protected virtual void OnEnable()
    {
        unitAnimation.Play(IdleAnimation);
        currentTarget = null;
        currentAttackCooldown = attackCooldown;
        commandIndex = UnitsManager.Instance.AddCommandToList(transform.position, detectionRaduis, transform.forward, detectionLayer);
        EventManager.OnResetUnitIndex += UpdateIndex;
    }

    private void UpdateIndex(int index)
    {
        if (commandIndex > index)
            commandIndex -= 1;
    }

    protected virtual void OnDisable()
    {
        UnitsManager.Instance.RemoveCommandFromList(commandIndex);
        EventManager.OnResetUnitIndex -= UpdateIndex;
    }

    protected virtual void Update()
    {
        if (TargetFinder.IsTargetActive(currentTarget))
            AttackTarget();
        else if (HasIdleUpdate)
            Idle();

        currentAttackCooldown -= Time.deltaTime;
        SimulatePhysics();
    }

    protected virtual void SimulatePhysics()
    {
        if (!TargetFinder.IsTargetActive(currentTarget))
            currentTarget = UnitsManager.Instance.GetTarget(commandIndex);

        UnitsManager.Instance.UpdateCommand(commandIndex, transform.position, transform.forward);
    }

    public virtual void PoolUnit() => PoolManager.Instance.AddToPool(poolID, gameObject);
    protected virtual void ResetCooldown() => currentAttackCooldown = attackCooldown;
    protected abstract void AttackTarget();
    protected abstract void Idle();
}