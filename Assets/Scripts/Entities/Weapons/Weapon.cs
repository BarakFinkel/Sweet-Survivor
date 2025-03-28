using UnityEngine;

public class Weapon : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Data")]
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }

    [Header("Level")]
    [field: SerializeField] public int Level { get; private set; } = 1;
    [SerializeField] public float levelStatFactor = 1.0f/3.0f;

    [Header("Attack Settings")]
    protected int damage = 5;
    [SerializeField] protected float baseAttackCooldown = 1.5f;
    protected float attackCooldown = 1.0f;
    protected float criticalHitMultiplier;
    protected int criticalHitChance;
    private float attackTimer = 0.0f;

    [Header("Enemy Detection Settings")]
    [SerializeField] protected float attackCheckRadius = 10.0f;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Animation Settings")]
    [SerializeField] protected Animator animator => GetComponentInChildren<Animator>();
    [SerializeField] protected float lerpFactor = 8.0f;

    protected void Start()
    {
        if (Level < 1)
        {
           throw new System.ArgumentOutOfRangeException("Error, level cannot be less than 1.");
        }
    }

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

    protected int CalculateDamage(out bool isCritHit)
    {
        if (Random.Range(0, 101) <= criticalHitChance)
        {
            isCritHit = true;
            return Mathf.RoundToInt(damage * criticalHitMultiplier);
        }
        else
        {
            isCritHit = false;
            return damage;
        }
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

    public virtual void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float levelStatMultiplier = 1 + levelStatFactor * (Level - 1);

        damage         = Mathf.RoundToInt( levelStatMultiplier * GetStatFromData(playerStatsManager, Stat.Attack) );
        attackCooldown = baseAttackCooldown / ( levelStatMultiplier * GetStatFromData(playerStatsManager, Stat.AttackSpeed) );

        criticalHitChance     = Mathf.RoundToInt( levelStatMultiplier * GetStatFromData(playerStatsManager, Stat.CriticalChance) );
        criticalHitMultiplier = levelStatMultiplier * GetStatFromData(playerStatsManager, Stat.CriticalDamage) / 100;
    }

    public float GetStatFromData(PlayerStatsManager playerStatsManager, Stat stat)
    {
        return WeaponData.GetStatValue(stat) + playerStatsManager.GetStatValue(stat);
    }
}