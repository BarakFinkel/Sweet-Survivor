using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private ShopItemContainer shopItemContainerPrefab;

    [Header("Shop Item Selection Settings")]
    [SerializeField] private int weaponContainerAmount  = 3;
    [SerializeField] private int objectContainerAmount  = 3;
    [SerializeField] private int maxPossibleWeaponLevel = 2;

    [Header("Reroll Settings")]
    [SerializeField] private Button RerollButton;
    [SerializeField] private TextMeshProUGUI rerollPriceText;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private int initialRerollPrice = 100;
    [SerializeField] private float rerollGrowthRate = 1.75f;
    private int rerollPrice;
    private int rerollCount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        GameManager.onGameStateChanged  += GameStateChangedCallback;
        ShopItemContainer.onTryPurchase += ItemPurchasedCallback;
        CurrencyManager.onUpdated       += CurrencyUpdatedCallback;

        rerollPrice = initialRerollPrice;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged  -= GameStateChangedCallback;
        ShopItemContainer.onTryPurchase -= ItemPurchasedCallback;
        CurrencyManager.onUpdated       -= CurrencyUpdatedCallback;
    }

    private void Configure()
    {
        containersParent.Clear();

        for (int i = 0; i < weaponContainerAmount; i++)
        {
            ShopItemContainer weaponContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);

            WeaponDataSO randomWeaponData = ResourceManager.GetRandomWeapon();
            weaponContainerInstance.Configure(randomWeaponData, Random.Range(1, maxPossibleWeaponLevel + 1));
        }

        for (int i = 0; i < objectContainerAmount; i++)
        {
            ShopItemContainer objectContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);

            ObjectDataSO randomObjectData = ResourceManager.GetRandomObject();
            objectContainerInstance.Configure(randomObjectData);
        }
    }

    # region Reroll

    public void Reroll()
    {     
        int oldRerollPrice = rerollPrice;
        
        rerollCount++;
        rerollPrice = Mathf.RoundToInt( Mathf.Pow(rerollGrowthRate, rerollCount) * initialRerollPrice );

        CurrencyManager.instance.UseCurrency(oldRerollPrice);

        Configure();
    }

    private void UpdateRerollVisuals()
    {
        rerollPriceText.text = rerollPrice.ToString();
        
        if (CurrencyManager.instance.HasEnoughCurrency(rerollPrice))
        {
            RerollButton.interactable = true;
            RerollButton.image.color  = activeColor;    
        }
        else
        {
            RerollButton.interactable = false;
            RerollButton.image.color  = inactiveColor; 
        }
    }
    # endregion

    # region Event Callbacks

    private void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            StartCoroutine(ConfigureDelayed());
            rerollCount = 0;
        }
    }

    private IEnumerator ConfigureDelayed()
    {
        yield return null;
        Configure();
        UpdateRerollVisuals();
    }

    private void ItemPurchasedCallback(ShopItemContainer container, int level)
    {
        if(container.purchaseItem is WeaponDataSO weaponData)
        {
            if (PlayerWeaponsManager.instance.TryAddWeapon(weaponData, level))
            {
                CurrencyManager.instance.UseCurrency(WeaponStatsCalculator.GetPurchasePrice(weaponData, level));

                Destroy(container.gameObject);
            }
        }
        else if (container.purchaseItem is ObjectDataSO objectData)
        {
            PlayerObjectsManager.instance.AddObject(objectData);
            CurrencyManager.instance.UseCurrency(objectData.Price);

            Destroy(container.gameObject);
        }
    }

    private void CurrencyUpdatedCallback()
    {
        UpdateRerollVisuals();
    }

    # endregion
}
