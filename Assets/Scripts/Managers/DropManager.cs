using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Random = UnityEngine.Random;

[System.Serializable]
public struct DropInfo
{
    public Drop dropPrefab;
    [Range(0f,1f)] public float dropChance;
}

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DropInfo[] dropDataArray;

    [Header("Dictionaries")]
    private Dictionary<Type, DropPoolBase> dropPools     = new Dictionary<Type, DropPoolBase>();
    private Dictionary<Type, float> dropChanceDictionary = new Dictionary<Type, float>();

    /// <summary>
    /// Subscribes to the onDeath event of the Enemy class.
    /// </summary>
    private void Awake()
    {
        Enemy.onDeath += EnemyDeathCallback;
    }

    /// <summary>
    /// Normalizes the drop chances and initializes the manager.
    /// </summary>
    private void Start()
    {
        NormalizeChances();
        InitializeManager();
    }

    /// <summary>
    /// Unsubscribes from the Enemy.onDeath event and cleans up drop pools.
    /// </summary>
    private void OnDisable()
    {
        Enemy.onDeath -= EnemyDeathCallback;

        // The DropPool class handles the event unsubscription automatically
        foreach (var dropPool in dropPools)
        {
            dropPool.Value.UnsubscribeFromEvent();  // Handle unsubscription within the pool
        }
    }

    /// <summary>
    /// Normalizes the drop chances so that their sum equals 1.
    /// This is used to ensure the chances are consistent.
    /// </summary>
    public void NormalizeChances()
    {
        float sum = 0f;
        
        foreach (var drop in dropDataArray)
            sum += drop.dropChance;

        if (sum > 0)
        {
            for (int i = 0; i < dropDataArray.Length; i++)
            {
                dropDataArray[i].dropChance /= sum;
            }
        }
    }

    /// <summary>
    /// Initializes the drop manager by setting up drop pools and validating drop chances.
    /// </summary>
    private void InitializeManager()
    {
        foreach (var dropData in dropDataArray)
        {
            if (dropData.dropPrefab == null)
            {
                Debug.LogError("Drop prefab is missing for a drop data entry.");
                continue;
            }

            Type dropType = dropData.dropPrefab.GetType();
            dropPools[dropType] = CreateDropPool(dropType, dropData.dropPrefab);  // Creating the pool
            dropChanceDictionary[dropType] = dropData.dropChance;
        }
    }

    /// <summary>
    /// Dynamically creates a drop pool for a given drop type using reflection.
    /// </summary>
    /// <param name="dropType">The type of the drop prefab.</param>
    /// <param name="dropPrefab">The drop prefab to be managed by the the pool.</param>
    /// <returns>A DropPoolBase instance for managing this type of drop.</returns>
    private DropPoolBase CreateDropPool(Type dropType, Drop dropPrefab)
    {
        var poolType = typeof(DropPool<>).MakeGenericType(dropType);                     // Dynamically create the DropPool type.
        return (DropPoolBase)Activator.CreateInstance(poolType, dropPrefab, transform);  // Create the pool instance.
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

    /// <summary>
    /// Determines the type of drop to spawn based on the weighted drop chances.
    /// </summary>
    /// <returns>The type of the selected drop.</returns>
    private Type GetRandomDropType()
    {
        float randomValue = Random.value; // Random float between 0 and 1
        float cumulativeChance = 0f;

        foreach (var entry in dropChanceDictionary)
        {
            cumulativeChance += entry.Value;
            if (randomValue <= cumulativeChance)
            {
                return entry.Key;
            }
        }

        return dropDataArray[0].dropPrefab.GetType(); // Fallback (should never happen if probabilities sum to 1)
    }

    #if UNITY_EDITOR
    
    /// <summary>
    /// A button to normalize the drop chances of all items currently in the DropInfo array.
    /// </summary>
    [CustomEditor(typeof(DropManager))]
    public class DropManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DropManager manager = (DropManager)target;
            if (GUILayout.Button("Normalize Drop Chances"))
            {
                manager.NormalizeChances();
                EditorUtility.SetDirty(manager);
            }
        }
    }

    #endif
}