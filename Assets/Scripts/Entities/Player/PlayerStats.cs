using System;
using UnityEngine;

public enum Stat
{
    Attack,
    AttackSpeed,
    CriticalChance,
    CriticalDamage,
    MoveSpeed,
    MaxHealth,
    Range,
    HealthRegen,
    Armor,
    Luck,
    DodgeChance,
    Lifesteal
}

public class PlayerStats : MonoBehaviour
{
    public static string FormatStatName(Stat stat)
    {
        string statRawName = stat.ToString();
        string statFormatedName = "";

        for (int i = 0; i < statRawName.Length; i++)
        {
            if (i != 0 && char.IsUpper(statRawName[i]))
                statFormatedName += " ";

            statFormatedName += statRawName[i];
        }

        Debug.Log(statFormatedName);
        return statFormatedName;
    }
}
