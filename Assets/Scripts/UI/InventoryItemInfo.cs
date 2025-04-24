using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryItemInfo : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemMeltPrice;
    
    [Header("Colors")]
    [SerializeField] private Image container;
    [SerializeField] private Color meltPossibleColor;
    [SerializeField] private Color meltImpossibleColor;

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

        // Merge Button Configuration - will only be active if the weapon merger allows it.
        mergeButton.gameObject.SetActive(true);

        mergeButton.interactable = WeaponMerger.instance.CanMerge(_weapon);
        mergeButton.onClick.RemoveAllListeners();
        mergeButton.onClick.AddListener(WeaponMerger.instance.Merge);
        mergeButton.onClick.AddListener(AudioManager.instance.PlayButtonSound);

        if (mergeButton.interactable)
            mergeButton.image.color = meltPossibleColor;
        else
            mergeButton.image.color = meltImpossibleColor;        

        // Melt Button Configuration - will only be active if the player has more than 1 weapon.
        meltButton.interactable = PlayerWeaponsManager.instance.GetNumberOfWeapons() > 1;

        if (meltButton.interactable)
            meltButton.image.color = meltPossibleColor;
        else
            meltButton.image.color = meltImpossibleColor;
    }

    public void Configure(ObjectDataSO _objectData)
    {
        Configure
        (
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
        InventoryItemContainer.DeselectHighlightedContainer();
    }
}
