using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Transform statContainersParent;

    [SerializeField] private Button PurchaseButton;
    [SerializeField] private Color activePrice;
    [SerializeField] private Color inactivePrice;

    [Header("Purchase")]
    public ItemSO purchaseItem { get; private set; }
    private int weaponLevel;

    [Header("Actions")]
    public static Action<ShopItemContainer, int> onTryPurchase;

    private void Awake()
    {
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }

    private void OnDisable()
    {
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
    }

    public void Configure(WeaponDataSO _weaponData, int _level)
    {
        purchaseItem = _weaponData;
        weaponLevel  = _level;
        
        // Setting Sprite and Name
        itemIcon.sprite = _weaponData.Icon;
        itemName.text   = _weaponData.Name + "\n" + $" (lvl {_level}) ";
        itemName.color  = ColorHolder.GetLevelColor(_level);

        // Setting Item price
        int weaponPrice = WeaponStatsCalculator.GetPurchasePrice(_weaponData, _level);
        itemPrice.text  = weaponPrice.ToString();

        // Setting Stat Containers
        ConfigureStatsContainers(_weaponData.Stats);

        UpdatePurchaseButtonInteraction(weaponPrice);

        PurchaseButton.onClick.AddListener(TryPurchase);
    }

    public void Configure(ObjectDataSO _objectData)
    {
        purchaseItem = _objectData;
        
        itemIcon.sprite = _objectData.Icon;
        itemName.text = _objectData.Name;
        itemName.color = ColorHolder.GetLevelColor(_objectData.Rarity);
        itemPrice.text = _objectData.Price.ToString();

        ConfigureStatsContainers(_objectData.Stats);

        UpdatePurchaseButtonInteraction(_objectData.Price);

        PurchaseButton.onClick.AddListener(TryPurchase);
    }

    private void ConfigureStatsContainers(Dictionary<Stat, float> stats)
    {
        statContainersParent.Clear();
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }

    private void TryPurchase()
    {
        onTryPurchase?.Invoke(this, weaponLevel);
    }

    private void UpdatePurchaseButtonInteraction(int price)
    {
        if (CurrencyManager.instance.HasEnoughCurrency(price))
        {
            PurchaseButton.interactable = true;
            PurchaseButton.image.color = activePrice;
        }
        else
        {
            PurchaseButton.interactable = false;
            PurchaseButton.image.color = inactivePrice;
        }
    }

    private void CurrencyUpdatedCallback()
    {
        int itemPrice = purchaseItem.Price;
        
        if (purchaseItem is WeaponDataSO weaponData)
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(weaponData, weaponLevel);

        UpdatePurchaseButtonInteraction(itemPrice);
    }
}
