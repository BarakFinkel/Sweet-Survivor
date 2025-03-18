using System;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Enemy Settings")]
    [SerializeField] private Transform projectileSource;
    [SerializeField] private ProjectilesManager projectilesManager;
    [SerializeField] private float projectileVelocity = 5.0f;

    protected override void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < attackRange)
        {
            enemyMovement.DisableMovement();

            if (CanAttack())
                Attack();
        }
        else
        {
            enemyMovement.EnableMovement();
        }
    }

    protected override void Attack()
    {
        Vector2 direction = (player.GetCenterPoint() - (Vector2)projectileSource.position).normalized;
        projectilesManager.UseProjectile(transform.position, direction, projectileVelocity, damage, false);
    }
}
