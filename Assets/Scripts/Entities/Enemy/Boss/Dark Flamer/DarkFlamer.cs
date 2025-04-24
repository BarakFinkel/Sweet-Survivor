using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum State 
{ 
    Idle,
    Move,
    BasicAttack,
    FlameBarrage,
    SpawnMinions
}

public class DarkFlamer : Enemy
{
    [Header("Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [field: SerializeField] public SpriteRenderer exclamationMark;

    [Header("Melee Attack Settings")]
    [SerializeField] private float meleeAttackRange = 0.75f;
    [SerializeField] private float meleeAttackCooldown = 0.5f;
    private float meleeAttackTimer = 0.0f;

    [Header("Ranged Attack Settings")]
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] private Transform projectileSource;
    [field: SerializeField] public float ProjectileVelocity { get; private set; } = 5.0f;
    [field: SerializeField] public float ProjectileRange { get; private set; } = 15.0f;
    public ProjectilesManager ProjectilesManager { get; private set; }

    [Header("State Machine")]
    public BossStateMachine StateMachine { get; private set; } = new BossStateMachine();
    [field: SerializeField] public float CloseRange { get; private set; } = 7.5f;

    #region States

    public DarkFlamerIdleState IdleState { get; private set; }
    public DarkFlamerMoveState MoveState { get; private set; }
    public DarkFlamerHopState HopState { get; private set; }
    public DarkFlamerBasicAttackState BasicAttackState { get; private set; }
    public DarkFlamerFlameBarrageState FlameBarrageState { get; private set; }
    public DarkFlamerSpawnMinionsState SpawnMinionsState { get; private set; }

    #endregion

    [field: Header("Idle State Settings")]
    [Min(1.0f)] [field: SerializeField] public float MaxIdleDuration { get; private set; } = 2.0f;

    [field: Header("Move State Settings")]
    [Min(1.0f)] [field: SerializeField] public float MaxMoveDuration { get; private set; } = 1.5f;

    [field: Header("Hop State Settings")]
    [Min(1.0f)] [field: SerializeField] public float HopWarningDuration { get; private set; } = 0.5f;
    [Min(1.0f)] [field: SerializeField] public float HopSpeed { get; private set; } = 10.0f;

    [field: Header("Basic Attack State Settings")]
    [Min(1.0f)] [field: SerializeField] public float AttackDuration { get; private set; } = 1.5f;

    [field: Header("Flame Barrage Settings")]
    [Min(1.0f)] [field: SerializeField] public float FlameBarrageDuration { get; private set; } = 3.0f;
    [field: SerializeField] public float FlameBarrageInterval { get; private set; } = 0.5f;

    [field: Header("Minions Spawn State Settings")]
    [field: SerializeField] public float MinionsSpawnDuration { get; private set; }
    [field: SerializeField] public GameObject MinionProjectile { get; private set; }
    public ProjectilesManager MinionProjectilesManager { get; private set; }
    [Min(1.0f)] [field: SerializeField] public float MinionProjectileVelocity { get; private set; } = 8.0f;

    protected override void Awake()
    {
        base.Awake();

        healthBar.gameObject.SetActive(false);

        StateMachine = new BossStateMachine();
        InitializeStates();

        onSpawnComplete += SpawnCompleteCallback;
        onDamageTaken   += DamageTakenCallback;
    }

    protected override void Start()
    {
        base.Start();
        
        StateMachine.Initiallize(IdleState);
        ProjectilesManager       = ProjectilesManager.FindProjectileManager(projectilePrefab);
        MinionProjectilesManager = ProjectilesManager.FindProjectileManager(MinionProjectile);
    }

    protected override void Update()
    {
        if (!hasSpawned)
            return;

        StateMachine.CurrentState.Update();

        UpdateAttackTimer();
        TryMeleeAttack();
    }

    private void OnDestroy()
    {
        onSpawnComplete -= SpawnCompleteCallback;
        onDamageTaken   -= DamageTakenCallback;
    }

    private void InitializeStates()
    {
        IdleState         = new DarkFlamerIdleState(this, StateMachine, this);
        MoveState         = new DarkFlamerMoveState(this, StateMachine, this);
        HopState          = new DarkFlamerHopState(this, StateMachine, this);
        BasicAttackState  = new DarkFlamerBasicAttackState(this, StateMachine, this);
        FlameBarrageState = new DarkFlamerFlameBarrageState(this, StateMachine, this);
        SpawnMinionsState = new DarkFlamerSpawnMinionsState(this, StateMachine, this);
    }

    # region Attack

    public override void Attack()
    {
        Vector2 direction = (player.GetCenterPoint() - (Vector2)projectileSource.position).normalized;
        ProjectilesManager.UseProjectile(transform.position, direction, ProjectileVelocity, ProjectileRange, 1, damage, false);
        PlayAttackSound();
    }

    public void TryMeleeAttack()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (distanceToPlayer < meleeAttackRange && CanMeleeAttack())
            Attack();
    }

    private bool CanMeleeAttack()
    {
        if (meleeAttackTimer == 0)
        {
            meleeAttackTimer = meleeAttackCooldown;
            return true;
        }
        
        return false;
    }

    # endregion

    # region Health Visuals

    private void SpawnCompleteCallback()
    {
        UpdateHealthBar();
        healthBar.gameObject.SetActive(true);
    }

    private void DamageTakenCallback(Transform transform, int damage, bool isCritHit)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = (float)health / maxHealth;
        healthText.text = health + " / " + maxHealth;
    }

    # endregion

    protected override void UpdateAttackTimer()
    {
        base.UpdateAttackTimer();

        if (meleeAttackTimer > 0)
            meleeAttackTimer = Mathf.Max(meleeAttackTimer - Time.deltaTime, 0);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
    }
}
