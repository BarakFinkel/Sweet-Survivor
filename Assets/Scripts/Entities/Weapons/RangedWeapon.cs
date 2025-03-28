using System;
using System.Collections;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Projectile Manager")]
    private ProjectilesManager projectilesManager => GetComponent<ProjectilesManager>();

    [Header("Projectile Settings")]
    [SerializeField] private float projectileVelocity;
    [SerializeField] private float attackDelay = 0.1f; // Delay before shooting after locking onto a target.
    private float range;

    [Header("Ranged Weapon Visuals")]
    [SerializeField] private bool applyingThrowEffect = true;
    [SerializeField] private float rendererReactivationDelay = 0.5f;
    private SpriteRenderer sr => GetComponentInChildren<SpriteRenderer>();

    protected override void HandleAttack()
    {
        UpdateAttackTimer();
        
        Transform closestEnemy = ClosestEnemy();
        
        if (closestEnemy != null && CanAttack())
            StartCoroutine(ShootWithDelay(closestEnemy, attackDelay));
    }

    private IEnumerator ShootWithDelay(Transform enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if(enemy != null)
            Shoot(enemy);
    }

    private void Shoot(Transform enemy)
    {
        Vector2 direction = (enemy.position - transform.position).normalized;
        projectilesManager.UseProjectile(transform.position, direction, projectileVelocity, range, CalculateDamage(out bool isCritHit), isCritHit);

        if (applyingThrowEffect)
            StartCoroutine(ToggleRendererWithDelay(rendererReactivationDelay));
    }

    private IEnumerator ToggleRendererWithDelay(float delay)
    {
        sr.enabled = false;
        yield return new WaitForSeconds(delay);
        sr.enabled = true;
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        base.UpdateStats(playerStatsManager);

        range = GetStatFromData(playerStatsManager, Stat.Range);
        attackCheckRadius = range;
    }
}
