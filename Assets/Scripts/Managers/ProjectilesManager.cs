using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilesManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] private Transform parent;

    [Header("Pooling")]
    private ObjectPool<Projectile> projectilePool;

    private void Start()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Method to instantiate a projectile object
    private Projectile CreateProjectile()
    {
        return Instantiate(projectilePrefab, transform).GetComponent<Projectile>();
    }

    // Method to configure projectile when getting a damage text from the pool
    private void ActionOnGet(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.transform.parent = parent;
    }

    // Method for releasing projectiles.
    private void ActionOnRelease(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectile.transform.parent = transform;
    }

    // Method for destruction of projectiles.
    private void ActionOnDestroy(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    // Gets a projectile instance and sets it according to the given parameters.
    public void UseProjectile(Vector2 source, Vector2 direction, float velocity, float range, int damage, bool isCritHit)
    {
        Projectile projectileInstance = projectilePool.Get();
        projectileInstance.SetupProjectile(this, source, direction, velocity, damage, isCritHit);

        // We release the projectile when we reach desired range.
        StartCoroutine(ReleaseProjectileWithDelay(projectileInstance, range / velocity));
    }

    private IEnumerator ReleaseProjectileWithDelay(Projectile projectile, float t)
    {
        yield return new WaitForSeconds(t);
        ReleaseProjectile(projectile);
    }

    public void ReleaseProjectile(Projectile projectile)
    {
        if (projectile.gameObject.activeSelf)
            projectilePool.Release(projectile);
    }
}
