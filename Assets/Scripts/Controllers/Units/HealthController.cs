using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [Header("Components To Disable")]
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private new Collider collider;
    [SerializeField] private UnitController controller;
    [SerializeField] private Transform geometry;
    private Vector3 initialScale;
    private Vector3 targetScale;

    private int Health { get; set; }
    public bool IsDead => Health <= 0;

    private void Awake()
    {
        initialScale = geometry.localScale;
        targetScale = Vector3.zero;
    }

    private void OnEnable()
    {
        Health = maxHealth;
        ToggleComponentsState(true);
    }

    public void SetHealth(int multiplier)
    {
        Health = maxHealth * multiplier;
    }

    public void TakeHit(bool instantKill = false)
    {
        if (!gameObject.activeSelf || Health == 0)
            return;

        Health = instantKill ? 0 : Mathf.Clamp(Health - 1, 0, maxHealth);

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