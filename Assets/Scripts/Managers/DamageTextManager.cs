using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 2, 0);
    [SerializeField] private float duration = 1.0f;

    [Header("Pooling")]
    private ObjectPool<DamageTextEffect> damageTextPool;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyHitCallback;
    }

    private void Start()
    {
        damageTextPool = new ObjectPool<DamageTextEffect>(CreateDamageText, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Method to instantiate a damage text game object
    private DamageTextEffect CreateDamageText()
    {
        return Instantiate(damageTextPrefab, transform).GetComponent<DamageTextEffect>();
    }

    // Method to configure damage text when getting a damage text from the pool
    private void ActionOnGet(DamageTextEffect damageTextEffect)
    {
        damageTextEffect.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageTextEffect damageTextEffect)
    {
        damageTextEffect.transform.parent = transform;
        damageTextEffect.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(DamageTextEffect damageTextEffect)
    {
        Destroy(damageTextEffect.gameObject);
    }

    private void OnDisable()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
    }

    private void EnemyHitCallback(Transform enemy, int damage)
    {
        DamageTextEffect damageTextInstance = damageTextPool.Get();
        damageTextInstance.SetupDamageText(enemy, spawnOffset, damage);

        StartCoroutine(ReleaseTextWithDelay(damageTextInstance, duration));
    }

    private IEnumerator ReleaseTextWithDelay(DamageTextEffect damageText, float t)
    {
        yield return new WaitForSeconds(t);
        damageTextPool.Release(damageText);
    }
}
