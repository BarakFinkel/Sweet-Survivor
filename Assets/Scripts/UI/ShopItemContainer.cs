using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    public ItemDataSO purchaseItem { get; private set; }
    private int weaponLevel;

    [Header("Actions")]
    public static Action<ShopItemContainer, int> onTryPurchase;

    private void Awake()
    {
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
        WeaponMerger.onMerge += WeaponMergeCallback;
    }

    private void OnDisable()
    {
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
        WeaponMerger.onMerge -= WeaponMergeCallback;
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
        StatContainerManager.GenerateStatContainers(WeaponStatsCalculator.GetStats(_weaponData, _level), statContainersParent);

        UpdatePurchaseButtonInteraction(weaponPrice, true);

        PurchaseButton.onClick.AddListener(TryPurchase);
        PurchaseButton.onClick.AddListener(AudioManager.instance.PlayButtonSound);
    }

    public void Configure(ObjectDataSO _objectData)
    {
        purchaseItem = _objectData;
        
        itemIcon.sprite = _objectData.Icon;
        itemName.text = _objectData.Name;
        itemName.color = ColorHolder.GetLevelColor(_objectData.Rarity);
        itemPrice.text = _objectData.Price.ToString();

        // Setting Stat Containers
        StatContainerManager.GenerateStatContainers(_objectData.Stats, statContainersParent);

        UpdatePurchaseButtonInteraction(_objectData.Price, false);

        PurchaseButton.onClick.AddListener(TryPurchase);
        PurchaseButton.onClick.AddListener(AudioManager.instance.PlayButtonSound);
    }

    private void TryPurchase()
    {
        onTryPurchase?.Invoke(this, weaponLevel);
    }

    private void UpdatePurchaseButtonInteraction(int price, bool isWeapon)
    {
        if (CurrencyManager.instance.HasEnoughCurrency(CurrencyType.Normal, price) && (!isWeapon || PlayerWeaponsManager.instance.GetNumberOfWeapons() < 6))
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

    private void WeaponMergeCallback(Weapon weapon)
    {
        CurrencyUpdatedCallback();
    }

    private void CurrencyUpdatedCallback()
    {
        StartCoroutine(CurrencyUpdatedCallbackDelayed());
    }

    private IEnumerator CurrencyUpdatedCallbackDelayed()
    {
        yield return null;

        int itemPrice = purchaseItem.Price;
        
        if (purchaseItem is WeaponDataSO weaponData)
        {
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(weaponData, weaponLevel);
            UpdatePurchaseButtonInteraction(itemPrice, true);
        }
        else
        {
            UpdatePurchaseButtonInteraction(itemPrice, false);
        }
    }
}
