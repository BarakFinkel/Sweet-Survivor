using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PopUpTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject popUpText;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 2, 0);
    [SerializeField] private float duration = 1.0f;

    [Header("Pooling")]
    private ObjectPool<PopUpText> popUpTextPool;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyHitCallback;
        PlayerDamageHandler.onDodge += PlayerDodgeCallback;
    }

    private void Start()
    {
        popUpTextPool = new ObjectPool<PopUpText>(CreateDamageText, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private void OnDisable()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
    }

    // Method to instantiate a damage text game object
    private PopUpText CreateDamageText()
    {
        return Instantiate(popUpText, transform).GetComponent<PopUpText>();
    }

    // Method to configure damage text when getting a damage text from the pool
    private void ActionOnGet(PopUpText damageTextEffect)
    {
        damageTextEffect.gameObject.SetActive(true);
    }

    private void ActionOnRelease(PopUpText damageTextEffect)
    {
        damageTextEffect.transform.parent = transform;
        damageTextEffect.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(PopUpText damageTextEffect)
    {
        Destroy(damageTextEffect.gameObject);
    }

    private void EnemyHitCallback(Transform enemy, int damage, bool isCritHit)
    {
        PopUpText damageTextInstance = popUpTextPool.Get();
        damageTextInstance.SetupDamageText(enemy, spawnOffset, damage, isCritHit);

        StartCoroutine(ReleaseTextWithDelay(damageTextInstance, duration));
    }

    private void PlayerDodgeCallback(Transform player)
    {
        PopUpText dodgeTextInstance = popUpTextPool.Get();
        dodgeTextInstance.SetupDodgeText(player, spawnOffset);

        StartCoroutine(ReleaseTextWithDelay(dodgeTextInstance, duration));
    }

    private IEnumerator ReleaseTextWithDelay(PopUpText damageText, float t)
    {
        yield return new WaitForSeconds(t);
        popUpTextPool.Release(damageText);
    }
}
