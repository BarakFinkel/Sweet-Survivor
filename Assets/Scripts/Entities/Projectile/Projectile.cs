using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isTargetPlayer = true; // Otherwise, needs to damage the enemy. 
    private Rigidbody2D rb;
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetupProjectile(Vector2 _source, Vector2 _direction, float _velocity, int _damage)
    {
        damage = _damage;
        transform.position = _source;
        transform.right = _direction;
        rb.linearVelocity = _direction * _velocity;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isTargetPlayer && collider.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
            gameObject.SetActive(false);
        }
        else if (!isTargetPlayer && collider.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
