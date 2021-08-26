using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private const float DistanceThreshold = 0.25f * 0.25f;

    [SerializeField] private PoolID id;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
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
        Move();
        CheckDistance();
        CheckTarget();
    }

    protected virtual void Move()
    {
        Vector3 direction = target.transform.position - transform.position;
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

    private void CheckTarget()
    {
        if (!target.gameObject.activeSelf)
            AddToPool();
    }

    protected virtual void ApplyDamage()
    {
        target.TakeHit(damage);
    }

    protected void AddToPool()
    {
        PoolManager.Instance.AddToPool(id, gameObject);
    }
}