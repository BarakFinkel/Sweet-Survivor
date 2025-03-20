using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DropInfo
{
    public Drop dropPrefab;
    [Range(0,100)] public float dropChance;
}

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DropInfo[] dropDataArray;  // Use DropData instead of separate arrays

    [Header("Dictionaries")]
    private Dictionary<Type, DropPoolBase> dropPools = new Dictionary<Type, DropPoolBase>();
    private Dictionary<Type, float> dropChanceMap = new Dictionary<Type, float>();

    private void Awake()
    {
        Enemy.onDeath += EnemyDeathCallback;
    }

    private void Start()
    {
        InitializeManager();
    }

    private void OnDisable()
    {
        Enemy.onDeath -= EnemyDeathCallback;

        // The DropPool class handles the event unsubscription automatically
        foreach (var dropPool in dropPools)
        {
            dropPool.Value.UnsubscribeFromEvent();  // Handle unsubscription within the pool
        }
    }

    private void InitializeManager()
    {
        float totalChance = 0f;
        
        foreach (var dropData in dropDataArray)
        {
            if (dropData.dropPrefab == null)
            {
                Debug.LogError("Drop prefab is missing for a drop data entry.");
                continue;
            }

            Type dropType = dropData.dropPrefab.GetType();
            dropPools[dropType] = CreateDropPool(dropType, dropData.dropPrefab);  // Creating the pool
            dropChanceMap[dropType] = dropData.dropChance;
            totalChance += dropData.dropChance;
        }

        // If there's more then a 0.01f error margin, log error accordingly.
        if (Math.Abs(totalChance - 1f) > 0.01f)
        {
            Debug.LogError("Drop chances must sum to 1!");
        }
    }

    // Dynamically creates a drop type pool.
    private DropPoolBase CreateDropPool(Type dropType, Drop dropPrefab)
    {
        var poolType = typeof(DropPool<>).MakeGenericType(dropType);                     // Dynamically create the DropPool type
        return (DropPoolBase)Activator.CreateInstance(poolType, dropPrefab, transform);  // Create the pool instance
    }

    private void EnemyDeathCallback(Vector2 dropPosition)
    {
        Type selectedDropType = GetRandomDropType();

        if (dropPools.TryGetValue(selectedDropType, out DropPoolBase pool))
        {
            Drop drop = pool.Get();
            drop.transform.position = dropPosition;
            drop.gameObject.SetActive(true);
        }
    }

    private Type GetRandomDropType()
    {
        float randomValue = UnityEngine.Random.value; // Random float between 0 and 1
        float cumulativeChance = 0f;

        foreach (var entry in dropChanceMap)
        {
            cumulativeChance += entry.Value;
            if (randomValue <= cumulativeChance)
            {
                return entry.Key;
            }
        }

        return dropDataArray[0].dropPrefab.GetType(); // Fallback (should never happen if probabilities sum to 1)
    }
}