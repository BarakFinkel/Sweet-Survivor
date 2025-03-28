using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/New Character Data", order = 0)]
public class CharacterDataSO : ScriptableObject
{
    [Header("Settings")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }

    [Space]

    [Header("Base Stats")]
    [Min(0)] [SerializeField] private float attack;
    [Min(0)] [SerializeField] private float attackSpeed;
    [Min(0)] [SerializeField] private float criticalChance;
    [Min(0)] [SerializeField] private float criticalDamage;
    [Min(0)] [SerializeField] private float moveSpeed;
    [Min(0)] [SerializeField] private float maxHealth;
    [Min(0)] [SerializeField] private float range;
    [Min(0)] [SerializeField] private float healthRegen;
    [Min(0)] [SerializeField] private float armor;
    [Min(0)] [SerializeField] private float luck;
    [Min(0)] [SerializeField] private float dodgeChance;
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
                {Stat.MoveSpeed,      moveSpeed},
                {Stat.MaxHealth,      maxHealth},
                {Stat.Range,          range},
                {Stat.HealthRegen,    healthRegen},
                {Stat.Armor,          armor},
                {Stat.Luck,           luck},
                {Stat.DodgeChance,    dodgeChance},
                {Stat.Lifesteal,      lifeSteal}
            };
        }
        
        private set
        {

        }
    }
}
