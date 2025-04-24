using System;
using UnityEngine;

using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    protected Player player;
    protected EnemyMovement enemyMovement;
    protected Collider2D cd;

    [Header("General Settings")]
    [SerializeField] protected int maxHealth;
    protected int health;
    protected bool hasSpawned = false;

    [Header("Spawn Settings")]
    [SerializeField] protected bool delayedSpawn = true;
    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected float targetScaleFactor;
    [SerializeField] protected float scaleLoopDuration;
    [SerializeField] protected int numOfLoops;

    [Header("Attack Settings")]
    [field: SerializeField] public int damage { get; private set; }
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCooldown;
    private float attackTimer = 0;

    [Header("Visuals and Effects")]
    [SerializeField] protected SpriteRenderer sr;
    protected ParticleSystem deathEffect;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;
    [Range(0.0f,1.0f)] [SerializeField] private float clipVolume;
    private AudioSource audioSource;

    [Header("Actions")]
    public static Action<Transform, int, bool> onDamageTaken;
    public static Action<Vector2> onDeath;
    protected Action onSpawnComplete;

    [Header("Debug")]
    [SerializeField] protected bool showGizmos = true;

    protected virtual void Awake()
    {
        health = maxHealth;

        enemyMovement = GetComponent<EnemyMovement>();
        cd            = GetComponent<Collider2D>();
        deathEffect   = GetComponentInChildren<ParticleSystem>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = attackSound;
        audioSource.volume = clipVolume;
    }

    protected virtual void Start()
    {
        player = FindFirstObjectByType<Player>();
        enemyMovement.SetPlayer(player);

        if (player == null)
        {
            Debug.LogWarning("No player found, destroying enemy.");
            Destroy(gameObject);
        }

        ToggleSpritesVisibility(false);

        HandleSpawn();
    }

    protected virtual void Update()
    {
        if (!hasSpawned)
            return;

        UpdateAttackTimer();
        TryAttack();
    }

    #region Spawn

    private void HandleSpawn()
    {
        if (delayedSpawn)
        {
            Vector3 targetScale = spawnIndicator.transform.localScale * targetScaleFactor;
            LeanTween.scale(spawnIndicator.gameObject, targetScale, scaleLoopDuration)
                .setLoopPingPong(numOfLoops)
                .setOnComplete(SpawnSequenceComplete);
        }
        else
        {
            SpawnSequenceComplete();
        }
    }

    private void SpawnSequenceComplete()
    {
        ToggleSpritesVisibility(true);
        hasSpawned = true;
        cd.enabled = true;

        EnableMovement();

        onSpawnComplete?.Invoke();
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

    public bool CanAttack()
    {
        if (attackTimer == 0)
        {
            attackTimer = attackCooldown;
            return true;
        }
        else return false;
    }

    public virtual void Attack()
    {
        player.TakeDamage(damage);
        PlayAttackSound();
    }

    protected virtual void UpdateAttackTimer()
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
        
        onDamageTaken?.Invoke(transform, damage, isCritHit);

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        // Death effects
        deathEffect.transform.SetParent(null);
        deathEffect.Play();
        
        PlayDeathSound();

        PopUpText[] damageTexts = GetComponentsInChildren<PopUpText>();
        for (int i = 0; i < damageTexts.Length; i++)
        {
            if (damageTexts[i] != null)
                damageTexts[i].gameObject.transform.parent = null;
        }

        // Event invokation;
        onDeath?.Invoke(transform.position);

        Destroy(gameObject);
    }

    private void ToggleSpritesVisibility(bool visibility)
    {
        sr.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    public void PlayAttackSound()
    {
        if (!AudioManager.instance.IsSFXOn || audioSource.clip == null)
            return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        if (!AudioManager.instance.IsSFXOn)
            return;

        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }

    protected virtual void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void EnableMovement()  => enemyMovement.EnableMovement();
    public void DisableMovement() => enemyMovement.DisableMovement();
}
