using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private PoolID id;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    protected float distance;

    public HealthController Target { get; set; }

    void Update()
    {
        Move();
        CheckDistance();
        CheckTarget();
    }

    protected virtual void Move()
    {
        Vector3 direction = Target.transform.position - transform.position;
        distance = direction.sqrMagnitude;
        transform.position += direction.normalized * speed * Time.deltaTime;
        transform.forward = direction;
    }

    private void CheckDistance()
    {
        if (distance <= 1)
        {
            ApplyDamage();
            AddToPool();
        }
    }

    private void CheckTarget()
    {
        if (!Target.gameObject.activeSelf)
            AddToPool();
    }

    protected virtual void ApplyDamage()
    {
        Target.TakeHit(damage);
    }

    protected void AddToPool()
    {
        PoolManager.Instance.AddToPool(id, gameObject);
    }
}