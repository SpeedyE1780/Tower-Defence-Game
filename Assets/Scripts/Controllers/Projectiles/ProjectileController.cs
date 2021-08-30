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
    private Vector3 startingPosition;
    private float moveDuration;
    private float currentLifetime;

    protected virtual void OnEnable()
    {
        EventManager.OnGameEnded += AddToPool;
        startingPosition = transform.position;
        progress = 0;
        currentLifetime = 0;
    }

    protected virtual void OnDisable()
    {
        EventManager.OnGameEnded -= AddToPool;
    }

    void Update()
    {
        UpdateProgress();
        CheckTarget();
        UpdateTargetPosition();
        Move();
        CheckDistance();
    }

    private void UpdateProgress()
    {
        currentLifetime += Time.deltaTime;
        progress = speedCurve.Evaluate(currentLifetime / moveDuration);
    }

    private void UpdateTargetPosition()
    {
        if (target != null)
            lastTargetPosition = target.Position;
    }

    protected virtual void Move()
    {
        Vector3 direction = lastTargetPosition - transform.position;
        transform.position = Vector3.Lerp(startingPosition, lastTargetPosition, progress);

        //Check to prevent zero vector rotation error
        if (direction.sqrMagnitude > 0.1f)
            transform.forward = direction;
    }

    private void CheckDistance()
    {
        if (progress >= 1)
        {
            ApplyDamage();
            AddToPool();
        }
    }

    public virtual void SetTarget(HealthController newTarget, int mask)
    {
        target = newTarget;
        unitMask = mask;

        //Get duration to reach target
        moveDuration = (transform.position - newTarget.Position).magnitude / speed;
    }

    //Make sure that current target is still active
    private void CheckTarget()
    {
        if (target != null && !target.gameObject.activeSelf)
            target = null;
    }

    protected virtual void ApplyDamage()
    {
        if (target != null)
            target.TakeHit(damage);
    }

    protected void AddToPool()
    {
        PoolManager.Instance.AddToPool(id, gameObject);
    }
}