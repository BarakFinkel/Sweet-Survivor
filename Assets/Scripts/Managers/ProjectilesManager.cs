using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilesManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float duration = 5.0f;

    [Header("Pooling")]
    private ObjectPool<Projectile> projectilePool;

    private void Start()
    {
        projectilePool = new ObjectPool<Projectile>(CreateDamageText, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Method to instantiate a projectile object
    private Projectile CreateDamageText()
    {
        return Instantiate(projectilePrefab, transform).GetComponent<Projectile>();
    }

    // Method to configure projectile when getting a damage text from the pool
    private void ActionOnGet(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    // Method for releasing projectiles.
    private void ActionOnRelease(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    // Method for destruction of projectiles.
    private void ActionOnDestroy(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    // Gets a projectile instance and sets it according to the given parameters.
    public void CreateProjectile(Vector2 source, Vector2 direction, float velocity, int damage)
    {
        Projectile projectileInstance = projectilePool.Get();
        projectileInstance.SetupProjectile(this, source, direction, velocity, damage);

        StartCoroutine(ReleaseProjectileWithDelay(projectileInstance, duration));
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
