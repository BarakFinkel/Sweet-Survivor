using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestObjectContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image objectIcon;
    [SerializeField] private TextMeshProUGUI objectName;
    [SerializeField] private Transform statContainersParent;
    [SerializeField] private TextMeshProUGUI objectRecycleAmount;

    [field: SerializeField] public Button EquipButton { get; private set; }
    [field: SerializeField] public Button MeltButton  { get; private set; }

    public void Configure(ObjectDataSO _objectData)
    {
        objectIcon.sprite = _objectData.Icon;
        objectName.text = _objectData.Name;
        objectRecycleAmount.text = "Melt for : " + _objectData.RecyclePrice.ToString();
        objectName.color = ColorHolder.GetLevelColor(_objectData.Rarity);

        ConfigureStatsContainers(_objectData.stats);
    }

    private void ConfigureStatsContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }
}
