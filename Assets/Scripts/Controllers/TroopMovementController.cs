using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TroopMovementController : MonoBehaviour
{
    private const string speedParameter = "Speed";
    private const string motionParameter = "MotionSpeed";

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speed;
    [SerializeField] private bool useAnim;
    [SerializeField] private float troopRaduis;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float detectionCooldown;
    [SerializeField] private float attackCooldown;
    private float currentCooldown;
    private Animator anim;
    private EnemyController currentTarget;

    void Awake()
    {
        if (useAnim)
            anim = GetComponent<Animator>();

        agent.speed = speed;
    }

    private void OnEnable()
    {
        StartCoroutine(LookForTarget());
        if (useAnim)
            SetAnimator();
    }

    private void Update()
    {
        if (TargetFinder.IsTargetActive(currentTarget))
            ActivateTroop();
        else
        {
            if (useAnim)
                anim.SetFloat(speedParameter, 0);

            agent.SetDestination(transform.position);
        }

        currentCooldown -= Time.deltaTime;
    }

    private void SetAnimator()
    {
        anim.SetFloat(speedParameter, 2);
        anim.SetFloat(motionParameter, 1);
    }

    private IEnumerator LookForTarget()
    {
        while (true)
        {
            if (!TargetFinder.IsTargetActive(currentTarget))
                FindTarget();

            yield return new WaitForSeconds(detectionCooldown);
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void FindTarget()
    {
        currentTarget = null;
        Collider[] enemies = Physics.OverlapSphere(transform.position, troopRaduis, enemyLayers, QueryTriggerInteraction.Ignore);

        if (enemies.Length == 0)
            return;

        currentTarget = TargetFinder.GetNearestTarget(enemies, transform.position).GetComponent<EnemyController>();
    }

    private void ActivateTroop()
    {
        if (useAnim)
        {
            anim.SetFloat(speedParameter, speed);
        }

        agent.SetDestination(currentTarget.transform.position);

        if ((currentTarget.transform.position - transform.position).sqrMagnitude < 25 && currentCooldown < 0)
            AttackTarget();
    }

    private void AttackTarget()
    {
        currentTarget.TakeHit();
        currentCooldown = attackCooldown;
    }
}