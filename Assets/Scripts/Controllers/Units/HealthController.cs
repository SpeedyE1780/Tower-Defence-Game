using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [Header("Components To Disable")]
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private new Collider collider;
    [SerializeField] private UnitController controller;

    private int Health { get; set; }
    public bool IsDead => Health == 0;

    private void OnEnable()
    {
        Health = maxHealth;
        ToggleComponentsState(true);
    }

    public void SetHealth(int multiplier)
    {
        Health = maxHealth * multiplier;
    }

    public void TakeHit()
    {
        if (!gameObject.activeSelf || Health == 0)
            return;

        Health = Mathf.Clamp(Health - 1, 0, maxHealth);

        if (IsDead)
            StartCoroutine(KillPlayer());
    }

    IEnumerator KillPlayer()
    {
        ToggleComponentsState(false);
        yield return new WaitForSeconds(3);
        controller.PoolUnit();
    }

    private void ToggleComponentsState(bool state)
    {
        if (agent != null)
            agent.enabled = state;

        collider.enabled = state;
        controller.enabled = state;
    }
}