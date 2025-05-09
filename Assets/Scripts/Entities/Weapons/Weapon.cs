using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Data")]
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }

    [Header("Level")]
    [SerializeField] public static float levelStatFactor = 1.0f/3.0f;
    public int level { get; private set; }

    [Header("Attack Settings")]
    protected Dictionary<Stat, float> weaponStats;
    protected int damage;
    [SerializeField] protected float baseAttackCooldown = 1.5f;
    protected float attackCooldown;
    protected float criticalHitMultiplier;
    protected int criticalHitChance;
    private float lifeStealFactor;
    private float attackTimer = 0.0f;

    [Header("Enemy Detection Settings")]
    [SerializeField] protected float attackCheckRadius = 10.0f;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Animation Settings")]
    [SerializeField] protected Animator animator => GetComponentInChildren<Animator>();
    [SerializeField] protected float lerpFactor = 8.0f;

    [Header("Audio Settings")]
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = WeaponData.attackSound;
        audioSource.volume = WeaponData.attackSoundVolume;

        Enemy.onDamageTaken               += EnemyTookDamageCallback;
        PlayerStatsManager.onStatsChanged += UpdateStats;
    }
    
    public void ConfigureWeapon(int _level)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        level = _level;

        UpdateStats();
    }

    protected virtual void Update()
    {
        AimWeapon();
        HandleAttack();
    }

    private void OnDisable()
    {
        Enemy.onDamageTaken               -= EnemyTookDamageCallback;
        PlayerStatsManager.onStatsChanged -= UpdateStats;
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

    #region Stats

    public virtual void UpdateStats()
    {
        weaponStats = WeaponStatsCalculator.GetStats(WeaponData, level);
        
        damage                = Mathf.RoundToInt(CalculateStatValue(Stat.Attack));
        attackCooldown        = baseAttackCooldown * (1 - CalculateStatValue(Stat.AttackSpeed) / 100);
        criticalHitChance     = Mathf.RoundToInt(CalculateStatValue(Stat.CriticalChance));
        criticalHitMultiplier = (150.0f + CalculateStatValue(Stat.CriticalDamage) ) / 100;
        lifeStealFactor       = CalculateStatValue(Stat.Lifesteal) / 100;
    }

    protected float CalculateStatValue(Stat stat)
    {
        return weaponStats[stat] + PlayerStatsManager.instance.GetStatValue(stat);
    }

    #endregion

    public void Upgrade()
    {
        level++;
        UpdateStats();
    }

    public void EnemyTookDamageCallback(Transform enemy, int damage, bool delay)
    {
        if (lifeStealFactor == 0)
            return;

        int lifeStealValue = Mathf.RoundToInt(damage * lifeStealFactor);
        lifeStealValue = Mathf.Max(lifeStealValue, 1);

        PlayerHealth.instance.ApplyHeal(lifeStealValue);
    }

    public void PlayAttackSound()
    {
        if(!AudioManager.instance.IsSFXOn)
            return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        // Gizmo that displays the weapon's aggro range.
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackCheckRadius);
    }
}