using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private const string speedParameter = "Speed";
    private const string motionParameter = "MotionSpeed";
    private static PoolManager pool;
    private static Transform destination;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speed;
    [SerializeField] private int maxHits;
    [SerializeField] private int points;
    [SerializeField] private int coins;
    [SerializeField] private float enemyRaduis;
    [SerializeField] private LayerMask troopsLayers;
    [SerializeField] private float detectionCooldown;
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private int hits;
    private float currentCooldown;
    public bool useAnim;
    private HealthController currentTarget;

    public float Speed => speed;

    private void Awake()
    {
        if (pool == null)
            pool = GameObject.Find("EnemyPool").GetComponent<PoolManager>();
        if (destination == null)
            destination = GameObject.FindGameObjectWithTag("Respawn").transform;

        if (useAnim)
            anim = GetComponent<Animator>();

        agent.speed = speed;
        StartCoroutine(LookForTarget());
    }

    private void Update()
    {
        if (TargetFinder.IsTargetActive(currentTarget))
            ActivateTroop();
        else
        {
            if (agent.destination != destination.position)
                agent.SetDestination(destination.position);
        }

        currentCooldown -= Time.deltaTime;
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
        Collider[] enemies = Physics.OverlapSphere(transform.position, enemyRaduis, troopsLayers, QueryTriggerInteraction.Ignore);

        if (enemies.Length == 0)
            return;

        currentTarget = TargetFinder.GetNearestTarget(enemies, transform.position).GetComponent<HealthController>();
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
        hits++;

        if (hits == maxHits)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        agent.SetDestination(destination.position);
    }

    public void SetEnemyController()
    {
        hits = 0;
        if (useAnim)
        {
            anim.SetFloat(speedParameter, 2);
            anim.SetFloat(motionParameter, 1);
        }
    }

    private void OnDisable()
    {
        EventManager.RaiseEnemyDisabled();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
            pool.Pool(gameObject);
    }

    public void TakeHit()
    {
        hits++;
        if (hits == maxHits)
        {
            pool.Pool(gameObject);
            EventManager.RaiseEnemyKilled(points, coins);
        }
    }
}