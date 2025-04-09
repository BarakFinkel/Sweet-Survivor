using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator
{
    public static Dictionary<Stat, float> GetStats(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + Weapon.levelStatFactor * (level - 1);

        Dictionary<Stat, float> calculatedStats = new Dictionary<Stat, float>();

        foreach (KeyValuePair<Stat, float> pair in weaponData.Stats)
        {
            if (weaponData.Prefab.GetType() != typeof(RangedWeapon) && pair.Key == Stat.Range)
                continue;
            
            calculatedStats.Add(pair.Key, pair.Value * multiplier);
        }

        return calculatedStats;
    }

    public static int GetPurchasePrice(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + Weapon.levelStatFactor * (level - 1);
        return Mathf.RoundToInt(weaponData.Price * multiplier);
    }
}
