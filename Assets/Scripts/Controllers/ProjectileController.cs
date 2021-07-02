using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;
    private static PoolManager pool;
    Rigidbody rb;

    void Start()
    {
        if (pool == null)
            pool = GameObject.Find("ProjectilePool").GetComponent<PoolManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(PoolObject());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }

    IEnumerator PoolObject()
    {
        yield return new WaitForSeconds(lifeTime);
        if (gameObject.activeSelf)
            pool.Pool(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            pool.Pool(gameObject);
    }
}