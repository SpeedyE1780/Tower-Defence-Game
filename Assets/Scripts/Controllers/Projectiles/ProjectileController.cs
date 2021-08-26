using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private const float DistanceThreshold = 0.5f * 0.5f;

    [SerializeField] private PoolID id;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    protected Vector3 lastTargetPosition;
    protected float distance;
    protected HealthController target;
    protected int unitMask;

    private void OnEnable()
    {
        EventManager.OnGameEnded += AddToPool;
    }

    private void OnDisable()
    {
        EventManager.OnGameEnded -= AddToPool;
    }

    void Update()
    {
        CheckTarget();
        UpdateTargetPosition();
        Move();
        CheckDistance();
    }

    private void UpdateTargetPosition()
    {
        if (target != null)
            lastTargetPosition = target.Position;
    }

    protected virtual void Move()
    {
        Vector3 direction = lastTargetPosition - transform.position;
        distance = direction.sqrMagnitude;
        transform.position += direction.normalized * speed * Time.deltaTime;
        transform.forward = direction;
    }

    private void CheckDistance()
    {
        if (distance <= DistanceThreshold)
        {
            ApplyDamage();
            AddToPool();
        }
    }

    public void SetTarget(HealthController newTarget, int mask)
    {
        target = newTarget;
        unitMask = mask;
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