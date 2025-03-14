using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Origin Point Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float constYOffsetFromPlayer = 1.0f;
    [SerializeField] private float distanceFromPlayer = 1.25f;
    [SerializeField] private Vector2 deafultOffsetFromPlayer = new Vector2(1.25f, 0.25f);
    private Vector3 originPosition;

    [Header("Enemy Detection Settings")]
    [SerializeField] private float attackCheckRadius = 10.0f;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attack Settings")]
    [SerializeField] private Transform hitCheckTransform;
    [SerializeField] protected int damage = 5;
    [SerializeField] private float hitRadius;
    [SerializeField] private float attackCooldown = 1.0f;
    private float attackTimer = 0.0f;
    private bool canDamage = false;
    private List<Collider2D> enemiesHit = new List<Collider2D>();

    [Header("Animation Settings")]
    [SerializeField] private float lerpFactor = 12.0f;
    private Animator animator => GetComponentInChildren<Animator>();

    void Update()
    {
        UpdateOriginPosition();
        AimWeapon();
        HandleAttack();
    }

    #region Aim

    protected Transform ClosestEnemy()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(originPosition, attackCheckRadius, enemyMask);
        
        if (enemyColliders.Length == 0)
            return null;

        Transform closestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D collider in enemyColliders)
        {
            float currDistance = Vector2.Distance(originPosition, collider.transform.position);
            if (currDistance < minDistance)
            {
                minDistance = currDistance;
                closestEnemy = collider.transform;
            }
        }

        return closestEnemy;
    }
    
    private void AimWeapon()
    {
        Transform closestEnemy = ClosestEnemy();
        Vector2 targetPosition;

        if (closestEnemy != null)
        {
            // Calculate direction from the adjusted origin position to the enemy
            Vector2 directionToEnemy = (closestEnemy.position - originPosition).normalized;
            
            // Change the target position to be the desired radius towards the closest enemy.
            targetPosition = (Vector2)originPosition + directionToEnemy * distanceFromPlayer;

            // Change the object's desired up direction to be towards the enemy.
            transform.up = Vector3.Lerp(transform.up, directionToEnemy, Time.deltaTime * lerpFactor);
        }
        else
        {
            // Default position relative to the adjusted origin
            targetPosition = (Vector2)originPosition + deafultOffsetFromPlayer;

            // Reset rotation upwards
            transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.deltaTime * lerpFactor);
        }

        // Smoothly move the weapon to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpFactor);
    }

    private void UpdateOriginPosition()
    {
        originPosition = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + constYOffsetFromPlayer,
            playerTransform.position.z
        );
    }

    #endregion

    #region Attack

    protected virtual void HandleAttack()
    {
        UpdateAttackTimer();
        
        // If an enemy is within range and the attack is off-cooldown, trigger the attack animation.
        if (ClosestEnemy() != null && CanAttack())
            animator.SetTrigger("Attack");

        TryDamage();
    }

    protected void TryDamage()
    {
        if (canDamage)
        {
            Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(hitCheckTransform.position, hitRadius, enemyMask);

            for (int i = 0; i < enemyColliders.Length; i++)
            {
                if (!enemiesHit.Contains(enemyColliders[i]))
                {
                    enemyColliders[i].GetComponent<Enemy>().TakeDamage(damage);
                    enemiesHit.Add(enemyColliders[i]);
                }
            }
        }
    }

    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
        enemiesHit.Clear();
    }

    protected bool CanAttack()
    {
        if (attackTimer == 0)
        {
            attackTimer = attackCooldown;
            return true;
        }
        else return false;
    }

    protected void UpdateAttackTimer()
    {
        if (attackTimer > 0)
        {
            attackTimer = Mathf.Max(attackTimer - Time.deltaTime, 0);
        }
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        // Gizmo that displays the weapon's aggro range.
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(originPosition, attackCheckRadius);

        // Gizmo that displays the weapon's hitbox.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitCheckTransform.position, hitRadius);
    }
}