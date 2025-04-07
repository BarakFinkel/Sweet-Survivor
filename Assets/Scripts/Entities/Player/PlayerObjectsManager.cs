using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectsManager : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> ObjectDatas { get; private set; }
    [SerializeField] private PlayerStatsManager statsManager;

    void Start()
    {
        foreach(ObjectDataSO objectData in ObjectDatas)
            statsManager.AddObjectStats(objectData.stats);
    }

    public void AddObject(ObjectDataSO objectData)
    {
        ObjectDatas.Add(objectData);
        statsManager.AddObjectStats(objectData.stats);
    }
}
