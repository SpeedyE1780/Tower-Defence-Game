using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private const string PlayerTag = "Player";

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
            PoolManager.Instance.AddToPool(id, gameObject);
    }
}