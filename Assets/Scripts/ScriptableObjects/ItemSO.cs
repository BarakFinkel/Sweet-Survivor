using System.Collections.Generic;
using UnityEngine;

public class ItemDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public int RecyclePrice { get; private set; }

    [SerializeField] private StatData[] StatDatas;
    
    public Dictionary<Stat, float> Stats
    {
        get
        {
            Dictionary<Stat, float> stats = new Dictionary<Stat, float>();

            foreach(StatData statData in StatDatas)
                stats.Add(statData.stat, statData.value);

            return stats;
        }

        private set
        {}
    }
}
