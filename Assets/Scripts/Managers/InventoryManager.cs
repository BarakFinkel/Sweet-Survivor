using System;
using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    [Header("Elements")]
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private Transform shopInventoryItemsParent;
    [SerializeField] private Transform pauseInventoryItemsParent;
    [SerializeField] private InventoryItemInfo itemInfoPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        GameManager.onGameStateChanged        += GameStateChangedCallback;
        GameManager.onGamePaused              += Configure;
        PlayerWeaponsManager.onWeaponsChanged += Configure;
        PlayerObjectsManager.onObjectsChanged += Configure;
        WeaponMerger.onMerge                  += MergedWeaponCallback;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged        -= GameStateChangedCallback;
        GameManager.onGamePaused              -= Configure;
        PlayerWeaponsManager.onWeaponsChanged -= Configure;
        PlayerObjectsManager.onObjectsChanged -= Configure;
        WeaponMerger.onMerge                  -= MergedWeaponCallback;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            StartCoroutine(ConfigureDelayed());
    }

    private void Configure()
    {
        shopInventoryItemsParent.Clear();
        pauseInventoryItemsParent.Clear();

        Weapon[] weapons = PlayerWeaponsManager.instance.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
                continue;

            InventoryItemContainer shopContainer = Instantiate(inventoryItemContainer, shopInventoryItemsParent);
            shopContainer.Configure(weapons[i], i, () => ShowItemInfo(shopContainer));

            InventoryItemContainer pauseContainer = Instantiate(inventoryItemContainer, pauseInventoryItemsParent);
            pauseContainer.Configure(weapons[i], i, null);         
        }

        ObjectDataSO[] objectDatas = PlayerObjectsManager.instance.ObjectDatas.ToArray();

        foreach (ObjectDataSO objectData in objectDatas)
        {
            InventoryItemContainer shopContainer = Instantiate(inventoryItemContainer, shopInventoryItemsParent);
            shopContainer.Configure(objectData, () => ShowItemInfo(shopContainer));

            InventoryItemContainer pauseContainer = Instantiate(inventoryItemContainer, pauseInventoryItemsParent);
            pauseContainer.Configure(objectData, null);
        }
    }

    # region Item Info Configuration

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon, container.WeaponIndex);
        else if (container.ObjectData != null)
            ShowObjectInfo(container.ObjectData);
    }

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        itemInfoPanel.Configure(weapon);
        ShopUIManager.instance.ShowItemInfo();
        itemInfoPanel.meltButton.onClick.RemoveAllListeners();
        itemInfoPanel.meltButton.onClick.AddListener(AudioManager.instance.PlayButtonSound);
        itemInfoPanel.meltButton.onClick.AddListener(() => MeltWeapon(index));
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfoPanel.Configure(objectData);
        itemInfoPanel.meltButton.onClick.RemoveAllListeners();
        itemInfoPanel.meltButton.onClick.AddListener(AudioManager.instance.PlayButtonSound);
        itemInfoPanel.meltButton.onClick.AddListener(() => MeltObject(objectData));

        ShopUIManager.instance.ShowItemInfo();
    }

    # endregion

    # region Item Info Buttons Methods

    private void MeltWeapon(int index)
    {
        PlayerWeaponsManager.instance.MeltWeapon(index);
        StartCoroutine(ConfigureDelayed());
        ShopUIManager.instance.HideItemInfo();
    }

    private void MeltObject(ObjectDataSO objectData)
    {
        PlayerObjectsManager.instance.MeltObject(objectData);
        StartCoroutine(ConfigureDelayed());
        ShopUIManager.instance.HideItemInfo();
    }

    private void MergedWeaponCallback(Weapon mergedWeapon)
    {
        Configure();
        StartCoroutine(ItemInfoConfigureDelayed(mergedWeapon));
    }

    # endregion

    # region Configuration Coroutines

    private IEnumerator ConfigureDelayed()
    {
        yield return null;
        Configure();
    }

    private IEnumerator ItemInfoConfigureDelayed(Weapon mergedWeapon)
    {
        yield return null;
        itemInfoPanel.Configure(mergedWeapon);
    }

    # endregion
}
