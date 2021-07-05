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
    private Animator anim;
    private int hits;
    public bool useAnim;

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