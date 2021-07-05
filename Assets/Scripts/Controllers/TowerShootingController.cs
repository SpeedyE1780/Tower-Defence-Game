using System.Collections;
using UnityEngine;

public class TowerShootingController : MonoBehaviour
{
    private static PoolManager projectilePool;

    [SerializeField] private Transform turret;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float detectionCooldown;
    [SerializeField] private ParticleSystem bulletCasing;
    [SerializeField] private float towerRaduis;
    [SerializeField] private LayerMask enemyLayers;
    [Range(0, 1)]
    [SerializeField] private float rotationSpeed;
    [Range(0, 1)]
    [SerializeField] private float hitChance;

    private float currentCooldown;
    private EnemyController currentTarget;
    private Vector3 targetForward;

    private void Start()
    {
        if (projectilePool == null)
            projectilePool = GameObject.Find("ProjectilePool").GetComponent<PoolManager>();

        targetForward = new Vector3();
        UpdateParticleDuration(bulletCasing);
        StartCoroutine(LookForTarget());
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentTarget != null && currentTarget.gameObject.activeInHierarchy)
            ActivateTurret();
        else
            bulletCasing.Stop();

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
        Collider[] enemies = Physics.OverlapSphere(transform.position, towerRaduis, enemyLayers, QueryTriggerInteraction.Ignore);

        if (enemies.Length == 0)
            return;

        currentTarget = TargetFinder.GetNearestTarget(enemies, transform.position).GetComponent<EnemyController>();
    }

    private void ActivateTurret()
    {
        RotateTurret();
        Shoot();
    }

    private void RotateTurret()
    {
        targetForward = currentTarget.transform.position - transform.position;
        turret.rotation = Quaternion.Slerp(turret.rotation, Quaternion.LookRotation(targetForward, Vector3.up), rotationSpeed);
    }

    private void Shoot()
    {
        if (currentCooldown > 0)
            return;

        SpawnProjectile();
        currentCooldown = shootCooldown;
        bulletCasing.Play();

        if (Random.Range(0, 1f) < hitChance)
            currentTarget.TakeHit();
    }

    private void SpawnProjectile()
    {
        GameObject projectile = projectilePool.Spawn();
        projectile.SetActive(true);
        projectile.transform.position = shootPoint.position;
        projectile.transform.forward = shootPoint.forward;
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
        Gizmos.DrawWireSphere(transform.position, towerRaduis);
    }
}