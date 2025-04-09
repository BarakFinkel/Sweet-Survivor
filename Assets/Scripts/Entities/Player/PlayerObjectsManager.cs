using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectsManager : MonoBehaviour
{
    public static PlayerObjectsManager instance;
    
    [field: SerializeField] public List<ObjectDataSO> ObjectDatas { get; private set; }
    [SerializeField] private PlayerStatsManager statsManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        foreach(ObjectDataSO objectData in ObjectDatas)
            statsManager.AddObjectStats(objectData.Stats);
    }

    public void AddObject(ObjectDataSO objectData)
    {
        ObjectDatas.Add(objectData);
        statsManager.AddObjectStats(objectData.Stats);
    }
}
