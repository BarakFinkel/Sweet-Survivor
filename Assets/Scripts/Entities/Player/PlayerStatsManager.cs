using System;
using System.Linq;
using System.Collections.Generic;
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

/// <summary>
/// A struct that hold multiple data fields over a specific instance of a stat type.
/// Currently neglected for simplification of the code.
/// </summary>
public struct StatData
{
    public Stat stat;
    public float value;

    public StatData(Stat _stat, float _value)
    {
        stat = _stat;
        value = _value;
    }

    public static StatData operator + (StatData data1, StatData data2)
    {
        if (data1.stat != data2.stat)
            throw new InvalidOperationException("Invalid operation - non-matching stat types.");
        
        float result = data1.value + data2.value;

        return new StatData(data1.stat, result);
    }

    public static StatData operator - (StatData data1, StatData data2)
    {
        return data1 + new StatData(data2.stat, -data2.value);
    }
}

public interface IPlayerStatsDependency
{
    void UpdateStats(PlayerStatsManager playerStatsManager);
}

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Instance")]
    public static PlayerStatsManager instance;

    [Header("Data")]
    [SerializeField] private CharacterDataSO playerData;

    [Header("Settings")]
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InitializePlayerStats();
    }

    public void Start()
    {
        UpdatePlayerStats();
    }

    public void AddToStat(Stat stat, float value)
    {
        // Player -> Base Stats

        // Addends -> Upgrades in the Leveling Panel
        // Stat - value

        if (addends.ContainsKey(stat))
            addends[stat] += value;
        else
            Debug.Log($"The key '{stat}' not found. Check your code.");

        UpdatePlayerStats();

        // Objects -> List of Object stats
    }

    public float GetStatValue(Stat stat)
    {
        float value = playerStats[stat] + addends[stat];
        
        return value;
    }

    private void InitializePlayerStats()
    {
        playerStats = playerData.BaseStats;

        foreach(KeyValuePair<Stat, float> pair in playerStats)
            addends.Add(pair.Key, 0);
    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies = 
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IPlayerStatsDependency>();

        foreach (IPlayerStatsDependency dependency in playerStatsDependencies)
            dependency.UpdateStats(this);
    }

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
