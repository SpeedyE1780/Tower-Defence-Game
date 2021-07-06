using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private PoolID id;
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;
    Rigidbody rb;

    void Start() => rb = GetComponent<Rigidbody>();
    private void OnEnable() => StartCoroutine(PoolObject());
    void FixedUpdate() => rb.velocity = transform.forward * speed;

    IEnumerator PoolObject()
    {
        yield return new WaitForSeconds(lifeTime);
        if (gameObject.activeSelf)
            PoolManager.Instance.AddToPool(id, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PoolManager.Instance.AddToPool(id, gameObject);
    }
}