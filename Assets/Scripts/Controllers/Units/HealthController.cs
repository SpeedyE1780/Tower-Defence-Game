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

        //Set health to max health
        UpdateHealth(currentMaxHealth);

        healthBar.gameObject.SetActive(false);
    }

    public void RaiseMaxHealth(float multiplier) => currentMaxHealth = Mathf.RoundToInt(maxHealth * multiplier);

    private void UpdateHealth(int health)
    {
        Health = Mathf.Clamp(health, 0, currentMaxHealth);
        healthBar.value = Mathf.Clamp01((float)health / currentMaxHealth);
    }

    public void TakeHit(bool instantKill = false)
    {
        if (!gameObject.activeSelf)
            return;

        if (!healthBar.gameObject.activeSelf)
            healthBar.gameObject.SetActive(true);

        UpdateHealth(instantKill ? 0 : Health - 1);

        if (IsDead)
            StartCoroutine(KillPlayer());
    }

    IEnumerator KillPlayer()
    {
        ToggleComponentsState(false);
        yield return StartCoroutine(PlayDeadAnimation());
        controller.PoolUnit();
    }

    IEnumerator PlayDeadAnimation()
    {
        float time = 0;
        float duration = 0.5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            geometry.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            yield return null;
        }
    }

    //Toggle movement and unit logic
    private void ToggleComponentsState(bool state)
    {
        geometry.localScale = initialScale;

        if (agent != null)
            agent.enabled = state;

        collider.enabled = state;
        controller.enabled = state;
    }
}