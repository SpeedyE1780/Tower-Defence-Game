using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopShootingController : MonoBehaviour
{
    private static PoolManager projectilePool;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float detectionCooldown;
    [SerializeField] private ParticleSystem bulletCasing;
    [SerializeField] private float troopRaduis;
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
            ShootTarget();
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
        Collider[] enemies = Physics.OverlapSphere(transform.position, troopRaduis, enemyLayers, QueryTriggerInteraction.Ignore);

        if (enemies.Length == 0)
            return;

        currentTarget = TargetFinder.GetNearestTarget(enemies, transform.position).GetComponent<EnemyController>();
    }

    private void ShootTarget()
    {
        Rotate();
        Shoot();
    }

    private void Rotate()
    {
        targetForward = currentTarget.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetForward, Vector3.up), rotationSpeed);
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
        Gizmos.DrawWireSphere(transform.position, troopRaduis);
    }
}