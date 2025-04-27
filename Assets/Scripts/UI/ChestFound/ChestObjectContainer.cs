using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI Container for details containing the treasure a chest drop contains.
/// Comes with functional buttons for either equipping the treasure or discarding for currency.
/// </summary>
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
        objectName.text = _objectData.Name + $" (lvl {_objectData.Rarity + 1})";
        objectRecycleAmount.text = "Melt for : " + _objectData.RecyclePrice.ToString();
        objectName.color = ColorHolder.GetLevelColor(_objectData.Rarity);

        ConfigureStatsContainers(_objectData.Stats);
    }

    private void ConfigureStatsContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }
}
