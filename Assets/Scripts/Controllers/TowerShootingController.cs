using UnityEngine;

public class TowerShootingController : MonoBehaviour
{
    private static PoolManager projectilePool;

    [SerializeField] private Transform turret;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown;
    [SerializeField] private ParticleSystem bulletCasing;
    [SerializeField] private float turretRaduis;
    [SerializeField] private LayerMask enemyLayers;
    [Range(0, 1)]
    [SerializeField] private float rotationSpeed;

    private float currentCooldown;
    private Transform currentTarget;
    private Vector3 targetForward;

    private void Start()
    {
        if (projectilePool == null)
            projectilePool = GameObject.Find("ProjectilePool").GetComponent<PoolManager>();

        targetForward = new Vector3();
        UpdateParticleDuration(bulletCasing);
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentTarget != null)
            ActivateTurret();
        else
            bulletCasing.Stop();

        currentCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        FindTarget();
    }

    protected virtual void FindTarget()
    {
        currentTarget = null;
        Collider[] enemies = Physics.OverlapSphere(transform.position, turretRaduis, enemyLayers, QueryTriggerInteraction.Ignore);

        if (enemies.Length == 0)
            return;

        float maxDistance = Mathf.Infinity;
        currentTarget = enemies[0].transform;
        Vector3 temp;

        for (int enemy = 1; enemy < enemies.Length; enemy++)
        {
            temp = transform.position - enemies[enemy].transform.position;
            if (temp.sqrMagnitude < maxDistance)
            {
                currentTarget = enemies[enemy].transform;
                maxDistance = temp.sqrMagnitude;
            }
        }

        targetForward = currentTarget.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetForward, Vector3.up), rotationSpeed);
    }

    private void ActivateTurret()
    {
        Shoot();
        bulletCasing.Play();
    }

    private void Shoot()
    {
        if (currentCooldown > 0)
            return;

        GameObject projectile = projectilePool.Spawn();
        projectile.transform.position = shootPoint.position;
        projectile.transform.forward = shootPoint.forward;
        currentCooldown = shootCooldown;
    }

    private void UpdateParticleDuration(ParticleSystem system)
    {
        ParticleSystem.MainModule main = system.main;
        main.duration = shootCooldown;

        foreach (ParticleSystem childSystem in system.GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.MainModule childMain = childSystem.main;
            childMain.duration = shootCooldown;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, turretRaduis);
    }
}