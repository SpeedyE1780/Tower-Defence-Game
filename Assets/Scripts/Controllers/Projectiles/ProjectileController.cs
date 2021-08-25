using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private PoolID id;
    [SerializeField] private float lifeTime;
    [SerializeField] private bool waitForLifeTime;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    protected Rigidbody rb;
    float currentLifetime;

    public HealthController Target { get; set; }

    void Start() => rb = GetComponent<Rigidbody>();
    private void OnEnable() => currentLifetime = lifeTime;

    void Update()
    {
        Move();

        if (waitForLifeTime)
            CheckLifetime();
    }

    protected virtual void Move()
    {
        rb.velocity = transform.forward * speed;
    }

    private void CheckLifetime()
    {
        currentLifetime -= Time.deltaTime;

        if (currentLifetime <= 0)
        {
            AddToPool();
            Target.TakeHit(damage);
        }
    }

    protected void AddToPool()
    {
        PoolManager.Instance.AddToPool(id, gameObject);
    }
}