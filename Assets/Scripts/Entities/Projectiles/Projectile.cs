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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public void SetupProjectile(ProjectilesManager _manager, Vector2 _source, Vector2 _direction, float _velocity, int _damage, bool _isCritHit)
    {
        // Set new parameter values for the current cycle.
        damage = _damage;
        myManager = _manager;
        transform.position = _source;
        transform.right = _direction;
        rb.linearVelocity = _direction * _velocity;
        
        isCritHit = _isCritHit;
        
        // Reset previous indicatiors before new cycle.
        target = null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
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

    // To check if a layer is within the layerMask using the bitwise-and operation.
    private bool IsInLayerMask(int layer)
    {
        return (targetLayerMask.value & (1 << layer)) != 0;
    }
}
