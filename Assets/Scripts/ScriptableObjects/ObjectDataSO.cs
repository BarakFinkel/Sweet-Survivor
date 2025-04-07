using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Data", menuName = "Scriptable Objects/New Object Data", order = 0)]
public class ObjectDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public int RecyclePrice { get; private set; }
    [field: Range(0,3)] [field: SerializeField] public int Rarity { get; private set; }

    [SerializeField] private StatData[] StatDatas;

    public Dictionary<Stat, float> stats
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
