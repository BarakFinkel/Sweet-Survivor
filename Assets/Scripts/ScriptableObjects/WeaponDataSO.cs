using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Objects/New Weapon Data", order = 1)]
public class WeaponDataSO : ItemDataSO
{
    [Header("Settings")]
    [field: SerializeField] public Weapon Prefab { get; private set; }
    [field: SerializeField] public AudioClip attackSound { get; private set; }
    [field: SerializeField] public float attackSoundVolume { get; private set; }

    public float GetStatValue(Stat stat)
    {
        if (Stats.ContainsKey(stat))
        {
            return Stats[stat];
        }
        else
        {
            Debug.LogError("WeaponData error - stat not found, getter returned 0.");
            return 0;
        }
    } 
}