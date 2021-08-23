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
    private void OnEnable() => StartCoroutine(PoolObject());
    void Update() => rb.velocity = transform.forward * speed;

    IEnumerator PoolObject()
    {
        currentLifetime = 0;
        while (currentLifetime < lifeTime)
        {
            currentLifetime += Time.deltaTime;
            yield return null;
        }

        if (gameObject.activeSelf)
            PoolManager.Instance.AddToPool(id, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
            PoolManager.Instance.AddToPool(id, gameObject);
    }
}