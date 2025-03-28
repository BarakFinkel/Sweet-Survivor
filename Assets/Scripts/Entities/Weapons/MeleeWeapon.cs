using UnityEngine;
using System.Collections.Generic;

public class MeleeWeapon : Weapon
{
    [Header("Attack Setings")]
    [SerializeField] private Transform hitCheckTransform;
    [SerializeField] private float hitRadius = 0.7f;
    
    [Header("Origin Point Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float constYOffsetFromPlayer = 1.0f;
    [SerializeField] private float distanceFromPlayer = 1.25f;
    [SerializeField] private Vector2 deafultOffsetFromPlayer = new Vector2(1.25f, 0.25f);
    private Vector3 originPosition;

    [Header("Hit Enemies Tracking Settings")]
    protected List<Collider2D> enemiesHit = new List<Collider2D>();

    [Header("Gateway Booleans")]
    protected bool canDamage = false;
    protected bool canAim = true;

    protected override void Update()
    {
        UpdateOriginPosition();
        AimWeapon();
        HandleAttack();
    }

    protected override void AimWeapon()
    {
        if(canAim)
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
    }

    private void UpdateOriginPosition()
    {
        originPosition = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + constYOffsetFromPlayer,
            playerTransform.position.z
        );
    }

    protected override void HandleAttack()
    {
        UpdateAttackTimer();
        
        // If an enemy is within range and the attack is off-cooldown, trigger the attack animation.
        if (ClosestEnemy() != null && CanAttack())
        {
            animator.SetTrigger("Attack");
        }

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
                    int dmg = CalculateDamage(out bool isCritHit);
                    enemyColliders[i].GetComponent<Enemy>().TakeDamage(dmg, isCritHit);
                    enemiesHit.Add(enemyColliders[i]);
                }
            }
        }
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        base.UpdateStats(playerStatsManager);

        animator.speed = Mathf.Clamp(1.0f + GetStatFromData(playerStatsManager, Stat.AttackSpeed) / 100, 1f, 2.5f);
    }

    #region Animation Trigger Methods

    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
        enemiesHit.Clear();
    }

    public void EnableAim()
    {
        canAim = true;
    }

    public void DisableAim()
    {
        canAim = false;
    }

    #endregion

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        // Gizmo that displays the weapon's hitbox.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitCheckTransform.position, hitRadius);
    }
}
