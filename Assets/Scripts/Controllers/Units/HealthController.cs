using System.Collections;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthBar;
    [Header("Components To Disable")]
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private new Collider collider;
    [SerializeField] private UnitController controller;
    [SerializeField] private Transform geometry;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private int currentMaxHealth;

    private int Health { get; set; }
    public bool IsDead => Health <= 0;

    private void Awake()
    {
        initialScale = geometry.localScale;
        targetScale = Vector3.zero;
        currentMaxHealth = maxHealth;
    }

    private void OnEnable()
    {
        ToggleComponentsState(true);
        UpdateHealth(currentMaxHealth);
    }

    public void UpdateMaxHealth(float multiplier)
    {
        currentMaxHealth = Mathf.RoundToInt(maxHealth * multiplier);
    }

    private void UpdateHealth(int health)
    {
        Health = health;
        healthBar.value = Mathf.Clamp01((float)health / currentMaxHealth);
    }

    public void TakeHit(bool instantKill = false)
    {
        if (!gameObject.activeSelf || Health == 0)
            return;

        UpdateHealth(instantKill ? 0 : Mathf.Clamp(Health - 1, 0, currentMaxHealth));

        if (IsDead)
            StartCoroutine(KillPlayer());
    }

    IEnumerator KillPlayer()
    {
        ToggleComponentsState(false);
        float time = 0;

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            geometry.localScale = Vector3.Lerp(initialScale, targetScale, time / 0.5f);
            yield return null;
        }

        controller.PoolUnit();
    }

    private void ToggleComponentsState(bool state)
    {
        geometry.localScale = initialScale;

        if (agent != null)
            agent.enabled = state;

        collider.enabled = state;
        controller.enabled = state;
    }
}