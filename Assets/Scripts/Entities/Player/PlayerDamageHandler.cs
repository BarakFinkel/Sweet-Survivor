using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlayerDamageHandler : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Elements")]
    private PlayerHealth playerHealth;
    
    [Header("Stats")]
    private float armorFactor;
    private float dodgeChance;
    private float lifeStealFactor;

    [Header("Actions")]
    public static Action<Transform> onDodge;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();

        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }

    private void OnDisable()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        armorFactor = playerStatsManager.GetStatValue(Stat.Armor) / 100;
        dodgeChance = playerStatsManager.GetStatValue(Stat.DodgeChance);
        lifeStealFactor = playerStatsManager.GetStatValue(Stat.Lifesteal) / 100;
    }

    public int CalculateDamage(int damage)
    {
        if (Random.Range(0, 100) < dodgeChance) // If dodging, no damage will be taken.
        {
            damage = 0;
            onDodge?.Invoke(transform);
        }
        else
        {
            float damageReductionFactor = Mathf.Clamp(1 - armorFactor, 0.5f, 1.0f);
            damage = Mathf.RoundToInt(damage * damageReductionFactor);
        }

        return damage;        
    }

    public void EnemyTookDamageCallback(Transform enemy, int damage, bool delay)
    {
        if (lifeStealFactor == 0)
            return;

        int lifeStealValue = Mathf.RoundToInt(damage * lifeStealFactor);
        lifeStealValue = Mathf.Max(lifeStealValue, 1);

        playerHealth.ApplyHeal(lifeStealValue);
    }
}
