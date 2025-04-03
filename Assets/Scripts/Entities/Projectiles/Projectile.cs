using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isTargetPlayer = true; // Otherwise, needs to damage the enemy.
    [SerializeField] private LayerMask targetLayerMask; 
    private ProjectilesManager myManager;
    private Rigidbody2D rb;
    private int damage;
    private Enemy target;
    private bool isCritHit;
    
    [Header("Timer Settings")]
    private float lifetime;
    private float timer = 0;
    private bool timerOn = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= lifetime)
        {
            timerOn = false;
            myManager.ReleaseProjectile(this);
        }
    }

    public void SetupProjectile(ProjectilesManager _manager, Vector2 _source, Vector2 _direction, float _velocity, float _lifetime ,int _damage, bool _isCritHit)
    {
        // Set new parameter values for the current cycle.
        damage = _damage;
        myManager = _manager;
        transform.position = _source;
        transform.right = _direction;
        rb.linearVelocity = _direction * _velocity;
        isCritHit = _isCritHit;
        
        // Set the new timer lifetime and turn it on.
        lifetime = _lifetime;
        timer = 0;
        timerOn = true;

        // Reset previous indicatiors before new cycle.
        target = null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (timerOn)
        {
            // Later we should use layer mask to get a more general way of detecting when to hit something.
            if (isTargetPlayer && collider.TryGetComponent(out Player player))
            {
                player.TakeDamage(damage);

                myManager.ReleaseProjectile(this);
            }
            else if (!isTargetPlayer && target == null && collider.TryGetComponent(out Enemy enemy))
            {
                target = enemy;
                enemy.TakeDamage(damage, isCritHit);
                rb.linearVelocity = Vector2.zero;
                
                myManager.ReleaseProjectile(this);
            }
        }
    }

    // To check if a layer is within the layerMask using the bitwise-and operation.
    private bool IsInLayerMask(int layer)
    {
        return (targetLayerMask.value & (1 << layer)) != 0;
    }
}
