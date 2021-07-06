using UnityEngine;

public class HealthController : MonoBehaviour
{
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

    private void OnEnable()
    {
        transform.localScale = initialScale;
    }

    public void TakeHit()
    {
        health = Mathf.Clamp(health - 1, 0, maxHealth);
        transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, (float)health / maxHealth);

        if (health <= 0)
            Destroy(gameObject);
    }
}