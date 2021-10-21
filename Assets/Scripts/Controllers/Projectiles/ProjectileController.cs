using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private PoolID id;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    protected Vector3 lastTargetPosition;
    protected HealthController target;
    protected int unitMask;
    protected float progress;
    protected bool targetDisabled;
    private Vector3 startingPosition;
    private float moveDuration;
    private float currentLifetime;

    private bool MovementIncomplete => progress < 1;

    #region UNITY MESSAGES

    protected virtual void OnEnable()
    {
        EventManager.OnGameEnded += AddToPool;
        ResetValues();
    }

    protected virtual void OnDisable() => EventManager.OnGameEnded -= AddToPool;

    private void Update()
    {
        UpdateProgress();
        UpdateTargetPosition();
        Move();
        CheckDestination();
    }

    #endregion

    #region TARGET FUNCTIONS

    public virtual void SetTarget(HealthController newTarget, int mask)
    {
        target = newTarget;
        lastTargetPosition = newTarget.Position;
        unitMask = mask;
        targetDisabled = false;

        //Get duration to reach target
        moveDuration = (transform.position - lastTargetPosition).magnitude / speed;
    }

    private void UpdateTargetPosition()
    {
        if (targetDisabled || IsTargetDisabled())
            return;

        lastTargetPosition = target.Position;
    }

    private bool IsTargetDisabled()
    {
        targetDisabled = target == null || !target.gameObject.activeSelf;
        return targetDisabled;
    }

    #endregion

    #region MOVEMENT

    //Move towards target last known position
    protected virtual void Move()
    {
        Vector3 direction = lastTargetPosition - transform.position;
        transform.position = Vector3.Lerp(startingPosition, lastTargetPosition, progress);

        //Check to prevent zero vector rotation error
        if (direction != Vector3.zero)
            transform.forward = direction;
    }

    #endregion

    #region UTILITY

    protected virtual void ResetValues()
    {
        startingPosition = transform.position;
        progress = 0;
        currentLifetime = 0;
    }

    private void UpdateProgress()
    {
        currentLifetime += Time.deltaTime;
        progress = speedCurve.Evaluate(currentLifetime / moveDuration);
    }

    private void CheckDestination()
    {
        if (MovementIncomplete)
            return;

        ApplyDamage();
        AddToPool();
    }

    protected virtual void ApplyDamage()
    {
        if (targetDisabled)
            return;

        target.TakeHit(damage);
    }

    protected void AddToPool() => PoolManager.Instance.AddToPool(id, gameObject);

    #endregion
}
