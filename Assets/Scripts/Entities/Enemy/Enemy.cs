using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    private Player player;
    private EnemyMovement enemyMovement;
    private Collider2D cd;

    [Header("General Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private TextMeshPro healthText;
    private int health;
    private bool hasSpawned = false;

    [Header("Spawn Settings")]
    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private float targetScaleFactor;
    [SerializeField] private float scaleLoopDuration;
    [SerializeField] private int numOfLoops;

    [Header("Attack Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    private float attackTimer = 0;

    [Header("Visuals and Effects")]
    [SerializeField] private SpriteRenderer sr;
    private ParticleSystem deathEffect;

    [Header("Actions")]
    public static Action<Transform, int> onDamageTaken;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;

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

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < attackRange && CanAttack())
        {
            Attack();
        }    
    }

    private bool CanAttack()
    {
        return attackTimer == 0;
    }

    void Attack()
    {
        Debug.Log("Dealing " + damage + " to the player!");
        SetAttackTimer();
        player.TakeDamage(damage);
    }

    private void SetAttackTimer()
    {
        attackTimer = attackCooldown;
    }

    private void UpdateAttackTimer()
    {
        attackTimer = Mathf.Max(attackTimer - Time.deltaTime, 0);
    }

    #endregion

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        healthText.text = health.ToString();
        
        onDamageTaken?.Invoke(transform, damage);

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
