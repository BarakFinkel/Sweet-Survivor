using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Enemy Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;

    protected override void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < attackRange && CanAttack())
        {
            enemyMovement.DisableMovement();
            Attack();
        }
        else
        {
            enemyMovement.EnableMovement();
        }
    }
    
    protected override void Attack()
    {
        
    }
}
