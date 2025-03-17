using System;
using System.Collections;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Projectile Settings")]
    [SerializeField] private float projectileVelocity;
    [SerializeField] private float attackDelay = 0.1f;
    private ProjectilesManager projectilesManager => GetComponent<ProjectilesManager>();

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

    private void Shoot(Transform enemy)
    {
        Vector2 direction = (enemy.position - sr.transform.position).normalized;
        projectilesManager.CreateProjectile(sr.transform.position, direction, projectileVelocity, damage);

        if (applyingThrowEffect)
            StartCoroutine(ToggleRendererWithDelay(rendererReactivationDelay));
    }

    private IEnumerator ShootWithDelay(Transform enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if(enemy != null)
            Shoot(enemy);
    }

    private IEnumerator ToggleRendererWithDelay(float delay)
    {
        sr.enabled = false;
        yield return new WaitForSeconds(delay);
        sr.enabled = true;
    }
}
