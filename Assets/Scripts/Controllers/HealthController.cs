using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeHit()
    {
        health -= 1;
        if (health <= 0)
            Destroy(gameObject);
    }
}