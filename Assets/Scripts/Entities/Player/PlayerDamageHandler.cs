using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class PlayerDamageHandler : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Stats")]
    private float armorFactor;
    private float dodgeChance;

    [Header("Actions")]
    public static Action<Transform> onDodge;

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        armorFactor = playerStatsManager.GetStatValue(Stat.Armor) / 100;
        dodgeChance = playerStatsManager.GetStatValue(Stat.DodgeChance);
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
}
