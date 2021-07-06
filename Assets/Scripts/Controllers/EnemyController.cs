using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private const string speedParameter = "Speed";
    private const string motionParameter = "MotionSpeed";
    private static Transform destination;

    [SerializeField] private PoolID id;
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
    private Vector3 initialScale;

    public float Speed => speed;

    private void Awake()
    {
        if (destination == null)
            destination = GameObject.FindGameObjectWithTag("Respawn").transform;

        if (useAnim)
            anim = GetComponent<Animator>();

        agent.speed = speed;
        initialScale = transform.localScale;
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
        transform.localScale = initialScale;
        agent.SetDestination(destination.position);
        StartCoroutine(LookForTarget());
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
            PoolManager.Instance.AddToPool(id, gameObject);
    }

    public void TakeHit()
    {
        hits++;
        transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, (float)hits / maxHits);
        if (hits == maxHits)
        {
            PoolManager.Instance.AddToPool(id, gameObject);
            EventManager.RaiseEnemyKilled(points, coins);
            transform.localScale = initialScale;
        }
    }
}