using System.Collections;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Projectile Elements")]
    [SerializeField] GameObject projectilePrefab;
    private ProjectilesManager projectilesManager;

    [Header("Projectile Settings")]
    [SerializeField] private float projectileVelocity;
    [SerializeField] private int numberOfTargets = 1;
    [SerializeField] private float attackDelay = 0.1f; // Delay before shooting after locking onto a target.
    private float range;

    [Header("Ranged Weapon Visuals")]
    [SerializeField] private bool applyingThrowEffect = true;
    [SerializeField] private float rendererReactivationDelay = 0.5f;
    [SerializeField] private SpriteRenderer sr;

    private Transform target;

    protected override void Awake()
    {
        base.Awake();
        projectilesManager = ProjectilesManager.FindProjectileManager(projectilePrefab);
    }

    protected override void AimWeapon()
    {
        Transform closestEnemy = ClosestEnemy();

        if (target != null)
            AimAt(target);
        
        else if (closestEnemy != null)
            AimAt(closestEnemy);

        else
            transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.deltaTime * lerpFactor);
    }

    private void AimAt(Transform target)
    {
        // Calculate the direction towards the enemy.
        Vector2 directionToEnemy = (target.position - transform.position).normalized;

        // Change the object's desired up direction to be towards the enemy.
        transform.up = Vector3.Lerp(transform.up, directionToEnemy, Time.deltaTime * lerpFactor);
    }

    protected override void HandleAttack()
    {
        UpdateAttackTimer();

        Transform closestEnemy = ClosestEnemy();

        if (closestEnemy != null && CanAttack())
            StartCoroutine(ShootWithDelay(closestEnemy, attackDelay));
    }

    private IEnumerator ShootWithDelay(Transform enemy, float delay)
    {
        target = enemy;
        
        yield return new WaitForSeconds(delay);
        
        if (enemy != null)
            Shoot(enemy);

        target = null;
    }

    private void Shoot(Transform enemy)
    {
        Vector2 direction = (enemy.position - transform.position).normalized;
        projectilesManager.UseProjectile(transform.position, direction, projectileVelocity, range, numberOfTargets, CalculateDamage(out bool isCritHit), isCritHit);
        PlayAttackSound();

        if (applyingThrowEffect)
            StartCoroutine(ToggleRendererWithDelay(rendererReactivationDelay));
    }

    private IEnumerator ToggleRendererWithDelay(float delay)
    {
        sr.enabled = false;
        yield return new WaitForSeconds(delay);
        sr.enabled = true;
    }

    public override void UpdateStats()
    {
        base.UpdateStats();

        range = CalculateStatValue(Stat.Range);
        attackCheckRadius = range;
    }
}
