using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    protected Player player;
    protected EnemyMovement enemyMovement;
    protected Collider2D cd;

    [Header("General Settings")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected TextMeshPro healthText;
    protected int health;
    protected bool hasSpawned = false;

    [Header("Spawn Settings")]
    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected float targetScaleFactor;
    [SerializeField] protected float scaleLoopDuration;
    [SerializeField] protected int numOfLoops;

    [Header("Attack Settings")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCooldown;
    private float attackTimer = 0;

    [Header("Visuals and Effects")]
    [SerializeField] protected SpriteRenderer sr;
    protected ParticleSystem deathEffect;

    [Header("Actions")]
    public static Action<Transform, int, bool> onDamageTaken;

    [Header("Debug")]
    [SerializeField] protected bool showGizmos = true;

    private void Start()
    {
        health = maxHealth;
        healthText.text = health.ToString();

        player = FindFirstObjectByType<Player>();
        
        enemyMovement = GetComponent<EnemyMovement>();
        enemyMovement.SetPlayer(player);

        cd = GetComponent<Collider2D>();

        deathEffect = gameObject.GetComponentInChildren<ParticleSystem>();

        if (player == null)
        {
            Debug.LogWarning("No player found, destroying enemy.");
            Destroy(gameObject);
        }
        
        ToggleSpritesVisibility(false);

        HandleSpawn();
    }

    private void Update()
    {
        if (!hasSpawned)
            return;

        UpdateAttackTimer();
        TryAttack();
    }

    #region Spawn

    private void HandleSpawn()
    {
        Vector3 targetScale = spawnIndicator.transform.localScale * targetScaleFactor;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, scaleLoopDuration)
            .setLoopPingPong(numOfLoops)
            .setOnComplete(SpawnSequenceComplete);
    }

    private void SpawnSequenceComplete()
    {
        ToggleSpritesVisibility(true);
        hasSpawned = true;
        cd.enabled = true;

        enemyMovement.EnableMovement();
    }

    #endregion

    #region Attack

    protected virtual void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < attackRange && CanAttack())
        {
            Attack();
        }    
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

    protected virtual void Attack()
    {
        Debug.Log("Dealing " + damage + " to the player!");
        player.TakeDamage(damage);
    }

    private void UpdateAttackTimer()
    {
        if (attackTimer > 0)
        {
            attackTimer = Mathf.Max(attackTimer - Time.deltaTime, 0);
        }
    }

    #endregion

    public void TakeDamage(int damage, bool isCritHit)
    {
        health = Mathf.Max(health - damage, 0);
        healthText.text = health.ToString();
        
        onDamageTaken?.Invoke(transform, damage, isCritHit);

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        deathEffect.transform.SetParent(null);
        deathEffect.Play();

        DamageTextEffect[] damageTexts = GetComponentsInChildren<DamageTextEffect>();
        for (int i = 0; i < damageTexts.Length; i++)
        {
            if (damageTexts[i] != null)
            {
                damageTexts[i].gameObject.transform.parent = null;
            }
        } 

        Destroy(gameObject);
    }

    private void ToggleSpritesVisibility(bool visibility)
    {
        sr.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
