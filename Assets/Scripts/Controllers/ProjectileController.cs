using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private PoolID id;
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;
    Rigidbody rb;
    float currentLifetime;

    void Start() => rb = GetComponent<Rigidbody>();
    private void OnEnable() => currentLifetime = lifeTime;

    void Update()
    {
        rb.velocity = transform.forward * speed;
        CheckLifetime();
    }

    private void CheckLifetime()
    {
        currentLifetime -= Time.deltaTime;

        if (currentLifetime <= 0)
            PoolManager.Instance.AddToPool(id, gameObject);
    }
}