using System;
using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    [Header("Elements")]
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private InventoryItemInfo itemInfoPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        GameManager.onGameStateChanged        += GameStateChangedCallback;
        PlayerWeaponsManager.onWeaponsChanged += Configure;
        PlayerObjectsManager.onObjectsChanged += Configure;
        WeaponMerger.onMerge                  += MergedWeaponCallback;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged        -= GameStateChangedCallback;
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
        inventoryItemsParent.Clear();

        Weapon[] weapons = PlayerWeaponsManager.instance.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
                continue;

            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(weapons[i], i, () => ShowItemInfo(container));
        }

        ObjectDataSO[] objectDatas = PlayerObjectsManager.instance.ObjectDatas.ToArray();

        foreach (ObjectDataSO objectData in objectDatas)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(objectData, () => ShowItemInfo(container));
        }
    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.weapon != null)
            ShowWeaponInfo(container.weapon, container.Index);
        else if (container.objectData != null)
            ShowObjectInfo(container.objectData);
    }

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        itemInfoPanel.Configure(weapon);
        ShopUIManager.instance.ShowItemInfo();
        itemInfoPanel.meltButton.onClick.RemoveAllListeners();
        itemInfoPanel.meltButton.onClick.AddListener(() => MeltWeapon(index));
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfoPanel.Configure(objectData);
        itemInfoPanel.meltButton.onClick.RemoveAllListeners();
        itemInfoPanel.meltButton.onClick.AddListener(() => MeltObject(objectData));

        ShopUIManager.instance.ShowItemInfo();
    }

    private void MeltWeapon(int index)
    {
        PlayerWeaponsManager.instance.MeltWeapon(index);
        Configure();
        ShopUIManager.instance.HideItemInfo();
    }

    private void MeltObject(ObjectDataSO objectData)
    {
        PlayerObjectsManager.instance.MeltObject(objectData);
        Configure();
        ShopUIManager.instance.HideItemInfo();
    }

    private IEnumerator ConfigureDelayed()
    {
        yield return null;
        Configure();
    }

    private void MergedWeaponCallback(Weapon mergedWeapon)
    {
        Configure();
        itemInfoPanel.Configure(mergedWeapon);
    }
}
