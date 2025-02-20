using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    private Player player;
    private EnemyMovement enemyMovement;

    [Header("General Settings")]
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

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        
        enemyMovement = GetComponent<EnemyMovement>();
        enemyMovement.SetPlayer(player);

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

    void Die()
    {
        deathEffect.transform.SetParent(null);
        deathEffect.Play();
        Destroy(gameObject);
    }

    void ToggleSpritesVisibility(bool visibility)
    {
        sr.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
