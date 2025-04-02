using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Enemy Settings")]
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] private Transform projectileSource;
    [SerializeField] private float projectileVelocity = 5.0f;
    [SerializeField] private float projectileRange = 10.0f;
    private ProjectilesManager projectilesManager;

    protected override void Start()
    {
        projectilesManager = ProjectilesManager.FindProjectileManager(projectilePrefab);
        base.Start();
    }

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
        projectilesManager.UseProjectile(transform.position, direction, projectileVelocity, projectileRange, damage, false);
    }
}
