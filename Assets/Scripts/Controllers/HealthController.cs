using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private PoolID poolID;
    [SerializeField] private int maxHealth;
    private int health;
    Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    void Start()
    {
        health = maxHealth;
    }

    public void TakeHit()
    {
        if (!gameObject.activeSelf)
            return;

        health = Mathf.Clamp(health - 1, 0, maxHealth);
        transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, (float)health / maxHealth);

        if (health <= 0)
        {
            transform.localScale = initialScale;
            PoolManager.Instance.AddToPool(poolID, gameObject);
        }
    }
}