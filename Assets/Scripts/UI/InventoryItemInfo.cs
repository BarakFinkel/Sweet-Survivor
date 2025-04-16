using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class InventoryItemInfo : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemMeltPrice;
    
    [Header("Colors")]
    [SerializeField] private Image container;

    [Header("Stats")]
    [SerializeField] private Transform statsParent;

    [Header("Buttons")]
    [field: SerializeField] public Button mergeButton { get; private set; }
    [field: SerializeField] public Button meltButton { get; private set; }

    public void Configure(Weapon _weapon)
    {
        Configure(
            _weapon.WeaponData.Icon,
            _weapon.WeaponData.Name + " lvl. " + _weapon.level,
            ColorHolder.GetLevelColor(_weapon.level),
            WeaponStatsCalculator.GetRecycleWorth(_weapon.WeaponData, _weapon.level),
            WeaponStatsCalculator.GetStats(_weapon.WeaponData, _weapon.level)
        );

        mergeButton.gameObject.SetActive(true);

        mergeButton.interactable = WeaponMerger.instance.CanMerge(_weapon);
        mergeButton.onClick.RemoveAllListeners();
        mergeButton.onClick.AddListener(WeaponMerger.instance.Merge);
    }

    public void Configure(ObjectDataSO _objectData)
    {
        Configure(
            _objectData.Icon,
            _objectData.Name,
            ColorHolder.GetLevelColor(_objectData.Rarity),
            _objectData.RecyclePrice,
            _objectData.Stats
        );

        mergeButton.gameObject.SetActive(false);
    }

    private void Configure(Sprite _icon, string _name, Color containerColor, int meltPrice, Dictionary<Stat, float> stats)
    {
        itemIcon.sprite    = _icon;
        itemName.text      = _name;
        itemName.color     = containerColor;
        itemMeltPrice.text = "+ " + meltPrice.ToString();
        container.color    = containerColor;
        
        statsParent.Clear();
        StatContainerManager.GenerateStatContainers(stats, statsParent);
    }

    public void DeselectHighlightedContainer()
    {
        if (InventoryItemContainer.lastSelectedContainer != null)
            InventoryItemContainer.lastSelectedContainer.DeselectHighlightedContainer();
    }
}
