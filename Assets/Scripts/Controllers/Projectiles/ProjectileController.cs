using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private PoolID id;
    [SerializeField] private float lifeTime;
    [SerializeField] private bool waitForLifeTime;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    float currentLifetime;

    public HealthController Target { get; set; }

    private void OnEnable() => currentLifetime = lifeTime;

    void Update()
    {
        Move();

        if (waitForLifeTime)
            CheckLifetime();
    }

    protected virtual void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void CheckLifetime()
    {
        currentLifetime -= Time.deltaTime;

        if (currentLifetime <= 0)
        {
            ApplyDamage();
            AddToPool();
        }
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