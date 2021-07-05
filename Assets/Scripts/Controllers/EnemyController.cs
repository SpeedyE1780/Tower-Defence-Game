using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const string speedParameter = "Speed";
    private const string motionParameter = "MotionSpeed";
    private static PoolManager pool;

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

        if (useAnim)
            anim = GetComponent<Animator>();
    }

    public void SetEnemyController()
    {
        EnemyManager.Instance.AddEnemy(transform);
        hits = 0;
        if (useAnim)
        {
            anim.SetFloat(speedParameter, 2);
            anim.SetFloat(motionParameter, 1);
        }
    }

    private void OnDisable()
    {
        if (EnemyManager.Instance)
            EnemyManager.Instance.RemoveEnemy(transform);

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