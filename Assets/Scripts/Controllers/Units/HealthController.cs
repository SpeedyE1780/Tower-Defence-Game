using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private static int DieParameter;

    [SerializeField] private int maxHealth;
    [SerializeField] private Animator anim;
    [Header("Components To Disable")]
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private new Collider collider;
    [SerializeField] private UnitController controller;

    private int Health { get; set; }
    public bool IsDead => Health == 0;

    [RuntimeInitializeOnLoadMethod]
    private static void SetDieParameter()
    {
        DieParameter = Animator.StringToHash("Die");
    }

    private void OnEnable() => Health = maxHealth;

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
        anim.SetTrigger(DieParameter);
        yield return new WaitForSeconds(3);
        ToggleComponentsState(true);
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