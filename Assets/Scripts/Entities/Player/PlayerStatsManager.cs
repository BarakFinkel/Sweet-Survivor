using System;
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
[System.Serializable]
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

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Instance")]
    public static PlayerStatsManager instance;
    public static Action onStatsChanged;

    [Header("Data")]
    [SerializeField] private CharacterDataSO playerData;

    [Header("Settings")]
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> levelingAddends = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> objectAddends = new Dictionary<Stat, float>();

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
        if (levelingAddends.ContainsKey(stat))
            levelingAddends[stat] += value;
        else
            Debug.Log($"The key '{stat}' not found. Check your code.");

        UpdatePlayerStats();
    }

    public float GetStatValue(Stat stat) => playerStats[stat] + levelingAddends[stat] + objectAddends[stat];

    private void InitializePlayerStats()
    {
        playerStats = playerData.BaseStats;

        foreach(KeyValuePair<Stat, float> pair in playerStats)
        {
            levelingAddends.Add(pair.Key, 0);
            objectAddends.Add(pair.Key, 0);
        }
    }

    public void AddObjectStats(Dictionary<Stat, float> objectStats) => AlterObjectStats(objectStats, true);

    public void RemoveObjectStats(Dictionary<Stat, float> objectStats) => AlterObjectStats(objectStats, false);

    public void AlterObjectStats(Dictionary<Stat, float> objectStats, bool isAdd)
    {
        foreach (KeyValuePair<Stat, float> pair in objectStats)
            objectAddends[pair.Key] += isAdd ? pair.Value : -pair.Value;

        UpdatePlayerStats();
    }

    private void UpdatePlayerStats()
    {
        onStatsChanged?.Invoke();
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

        return statFormatedName;
    }
}
