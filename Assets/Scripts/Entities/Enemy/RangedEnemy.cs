using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Enemy Settings")]
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] private Transform projectileSource;
    [SerializeField] private float projectileVelocity = 5.0f;
    private ProjectilesManager projectilesManager;

    protected override void Start()
    {
        FindProjectileManager();
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
        projectilesManager.UseProjectile(transform.position, direction, projectileVelocity, damage, false);
    }

    protected void FindProjectileManager()
    {
        ProjectilesManager[] managers = FindObjectsByType<ProjectilesManager>(FindObjectsSortMode.None);

        foreach (ProjectilesManager manager in managers)
        {
            if (projectilePrefab == manager.projectilePrefab)
            {
                projectilesManager = manager;
                break;
            }
        }

        if (projectilesManager == null)
            Debug.LogError("Error: No corresponding projectile manager found.");
    }
}
