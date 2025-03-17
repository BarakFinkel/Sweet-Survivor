using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isTargetPlayer = true; // Otherwise, needs to damage the enemy.
    [SerializeField] private LayerMask targetLayerMask; 
    private ProjectilesManager manager;
    private Rigidbody2D rb;
    private int damage;
    public bool released;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetupProjectile(ProjectilesManager _manager, Vector2 _source, Vector2 _direction, float _velocity, int _damage)
    {
        damage = _damage;
        manager = _manager;
        transform.position = _source;
        transform.right = _direction;
        rb.linearVelocity = _direction * _velocity;
        released = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Later we should use layer mask to get a more general way of detecting when to hit something.
        if (isTargetPlayer && collider.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
            manager.ReleaseProjectile(this);
        }
        else if (!isTargetPlayer && collider.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            manager.ReleaseProjectile(this);
        }
    }

    private bool IsInLayerMask(int layer)
    {
        return (targetLayerMask.value & (1 << layer)) != 0;
    }
}
