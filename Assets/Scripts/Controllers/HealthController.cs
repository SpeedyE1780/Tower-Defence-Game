using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private PoolID poolID;
    [SerializeField] private int maxHealth;
    Vector3 initialScale;
    public int Health { get; private set; }

    private void Awake() => initialScale = transform.localScale;
    private void OnEnable() => Health = maxHealth;
    private void OnDisable() => transform.localScale = initialScale;

    public void TakeHit()
    {
        if (!gameObject.activeSelf)
            return;

        Health = Mathf.Clamp(Health - 1, 0, maxHealth);
        transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, (float)Health / maxHealth);

        if (Health <= 0)
            PoolManager.Instance.AddToPool(poolID, gameObject);
    }
}