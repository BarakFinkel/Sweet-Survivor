using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectsManager : MonoBehaviour
{
    public static PlayerObjectsManager instance;
    
    [field: SerializeField] public List<ObjectDataSO> ObjectDatas { get; private set; }

    public static Action onObjectsChanged;

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
            PlayerStatsManager.instance.AddObjectStats(objectData.Stats);
    }

    public void AddObject(ObjectDataSO objectData)
    {
        ObjectDatas.Add(objectData);
        PlayerStatsManager.instance.AddObjectStats(objectData.Stats);

        onObjectsChanged?.Invoke();
    }

    public void MeltObject(ObjectDataSO objectData)
    {
        ObjectDatas.Remove(objectData);
        CurrencyManager.instance.AddCurrency(objectData.RecyclePrice);
        PlayerStatsManager.instance.RemoveObjectStats(objectData.Stats);

        onObjectsChanged?.Invoke();
    }
}
