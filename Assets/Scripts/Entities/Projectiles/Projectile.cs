using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isTargetPlayer = true; // Otherwise, needs to damage the enemy.
    [SerializeField] private LayerMask targetLayerMask;
    private ProjectilesManager myManager;
    public Rigidbody2D rb;
    private int damage;
    private bool isCritHit;

    [Header("Timer Settings")]
    private float lifetime;
    private float timer = 0;
    private bool timerOn = false;

    private List<Enemy> targets = new List<Enemy>();
    private int maxNumberOfTargets;
    private int targetsCounter = 0;

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

    public void SetupProjectile(ProjectilesManager _manager, Vector2 _source, Vector2 _direction, float _velocity, float _lifetime, int _numberOfTargets, int _damage, bool _isCritHit)
    {
        // Set new parameter values for the current cycle.
        damage = _damage;
        maxNumberOfTargets = _numberOfTargets;
        targetsCounter = 0;
        myManager = _manager;
        transform.position = _source;
        transform.right = _direction;
        rb.linearVelocity = _direction * _velocity;
        isCritHit = _isCritHit;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 45);

        // Set the new timer lifetime and turn it on.
        lifetime = _lifetime;
        timer = 0;
        timerOn = true;

        // Reset previous indicatiors before new cycle.
        targets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (timerOn)
        {
            // If the target is a player - the player will take damage and the object will be released.
            if (isTargetPlayer && collider.TryGetComponent(out Player player))
            {
                player.TakeDamage(damage);
                myManager.ReleaseProjectile(this);
            }
            // If the target is an enemy, and we collided with an enemy
            else if (!isTargetPlayer && collider.TryGetComponent(out Enemy enemy))
            {
                // If we found a new enemy and have targeted less than the max amount, we add it to the list and make it take damage.
                if (targetsCounter < maxNumberOfTargets && !targets.Contains(enemy))
                {
                    targets.Add(enemy);
                    enemy.TakeDamage(damage, isCritHit);

                    targetsCounter++;
                }

                // If we reached the maximum ammount of enemies to be hit, we release the projectile.
                if (targetsCounter == maxNumberOfTargets)             
                    myManager.ReleaseProjectile(this);
            }
        }
    }
}
