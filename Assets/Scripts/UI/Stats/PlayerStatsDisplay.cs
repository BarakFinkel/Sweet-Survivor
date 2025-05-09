using System;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform statContainersParent;

    private void Awake()
    {
        PlayerStatsManager.onStatsChanged += UpdateStats;
    }

    private void Start()
    {
        UpdateStats();   
    }

    private void OnDestroy()
    {
        PlayerStatsManager.onStatsChanged -= UpdateStats;
    }

    /// <summary>
    /// Update the stats UI based on the current player's stats.
    /// </summary>
    /// <param name="playerStatsManager">The stat manager from which the updated stats are taken.</param>
    public void UpdateStats()
    {
        int index = 0;

        // For each type of stat, configure a UI stat container for it within the stats display.
        foreach(Stat stat in Enum.GetValues(typeof(Stat)))
        {
            if (index >= statContainersParent.childCount)
                return;
            
            StatContainer statContainer = statContainersParent.GetChild(index).GetComponent<StatContainer>();
            statContainer.gameObject.SetActive(true);

            Sprite statIcon = ResourceManager.GetStatIcon(stat);
            float statValue = PlayerStatsManager.instance.GetStatValue(stat);

            statContainer.Configure(statIcon, PlayerStatsManager.FormatStatName(stat), statValue, true);

            index++;
        }

        // Disable unnecessary extra stat containers.
        for (int i = index; i < statContainersParent.childCount; i++)
        {
            statContainersParent.GetChild(index).gameObject.SetActive(false);
        }
    }
}
