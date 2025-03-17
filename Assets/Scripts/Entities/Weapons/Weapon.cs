using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Enemy Detection Settings")]
    [SerializeField] protected float attackCheckRadius = 10.0f;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Attack Settings")]
    [SerializeField] protected int damage = 5;
    [SerializeField] protected float attackCooldown = 1.0f;
    private float attackTimer = 0.0f;

    [Header("Animation Settings")]
    [SerializeField] protected float lerpFactor = 8.0f;

    protected virtual void Update()
    {
        AimWeapon();
        HandleAttack();
    }

    #region Aim

    protected Transform ClosestEnemy()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, attackCheckRadius, enemyMask);
        
        if (enemyColliders.Length == 0)
            return null;

        Transform closestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D collider in enemyColliders)
        {
            float currDistance = Vector2.Distance(transform.position, collider.transform.position);
            if (currDistance < minDistance)
            {
                minDistance = currDistance;
                closestEnemy = collider.transform;
            }
        }

        return closestEnemy;
    }

    protected virtual void AimWeapon()
    {
        Transform closestEnemy = ClosestEnemy();

        if (closestEnemy != null)
        {
            // Calculate the direction towards the enemy.
            Vector2 directionToEnemy = (closestEnemy.position - transform.position).normalized;

            // Change the object's desired up direction to be towards the enemy.
            transform.up = Vector3.Lerp(transform.up, directionToEnemy, Time.deltaTime * lerpFactor);
        }
        else
        {
            // Reset rotation upwards
            transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.deltaTime * lerpFactor);
        }
    }

    #endregion

    #region Attack

    protected virtual void HandleAttack() {}

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

    protected virtual void OnDrawGizmosSelected()
    {
        // Gizmo that displays the weapon's aggro range.
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackCheckRadius);
    }
}