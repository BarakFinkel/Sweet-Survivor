using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Objects/New Weapon Data", order = 1)]
public class WeaponDataSO : ScriptableObject
{
    [Header("Settings")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }
    [field: SerializeField] public Weapon Prefab { get; private set; }

    [Space]

    [Header("Base Stats")]
    [Min(0)] [SerializeField] private float attack;
    [Min(0)] [SerializeField] private float attackSpeed;
    [Min(0)] [SerializeField] private float criticalChance;
    [Min(0)] [SerializeField] private float criticalDamage;
    [Min(0)] [SerializeField] private float range;
    [Min(0)] [SerializeField] private float lifeSteal;

    public Dictionary<Stat, float> BaseStats
    { 
        get
        {
            return new Dictionary<Stat, float>
            {
                {Stat.Attack,         attack},
                {Stat.AttackSpeed,    attackSpeed},
                {Stat.CriticalChance, criticalChance},
                {Stat.CriticalDamage, criticalDamage},
                {Stat.Range,          range},
                {Stat.Lifesteal,      lifeSteal}
            };
        }

        private set
        {

        }
    }

    public float GetStatValue(Stat stat)
    {
        if (BaseStats.ContainsKey(stat))
        {
            return BaseStats[stat];
        }
        else
        {
            Debug.LogError("WeaponData error - stat not found, getter returned 0.");
            return 0;
        }
    } 
}