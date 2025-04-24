using UnityEngine;

public class MinionProjectile : Projectile
{
    [SerializeField] private GameObject minionPrefab;
    private Vector2 initialVelocity;
    private Vector2 deceleration;
    private bool canDamage;

    public override void SetupProjectile(ProjectilesManager _manager, Vector2 _source, Vector2 _direction, float _velocity, float _lifetime, int _numberOfTargets, int _damage, bool _isCritHit)
    {
        base.SetupProjectile(_manager, _source, _direction, _velocity, _lifetime, _numberOfTargets, _damage, _isCritHit);

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        initialVelocity = _direction * _velocity;
        deceleration = initialVelocity / _lifetime;
        canDamage = true;
    }

    protected override void Update()
    {
        base.Update();

        if (timer >= lifetime)
            Instantiate(minionPrefab, transform.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        if (!timerOn)
            return;

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero, deceleration.magnitude * Time.fixedDeltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        // If the target is a player - the player will take damage and the object will be released.
        if (isTargetPlayer && collider.TryGetComponent(out Player player))
        {
            if(canDamage)
            {
                player.TakeDamage(damage);
                canDamage = false;
            }
        }
    }
}
