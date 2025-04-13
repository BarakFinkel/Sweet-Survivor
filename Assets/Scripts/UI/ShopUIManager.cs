using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    public static ShopUIManager instance;
    
    [Header("Player Stats Elements")]
    [SerializeField] private RectTransform statsPanel;
    [SerializeField] private RectTransform statsBackground;
    private Vector2 statsPanelOpenedPos;
    private Vector2 statsPanelClosedPos;

    [Header("Inventory Elements")]
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryBackground;
    private Vector2 inventoryPanelOpenedPos;
    private Vector2 inventoryPanelClosedPos;

    [Header("Item Info Elements")]
    [SerializeField] private RectTransform itemInfoPanel;
    private Vector2 itemInfoOpenedPos;
    private Vector2 itemInfoClosedPos;

    [Header("Slide Settings")]
    [SerializeField] private float desiredWidthRation = 1.0f/3.0f;
    [SerializeField] private float desiredHeightRation = 1.0f/2.0f;
    [SerializeField] private float slideTime = 0.5f;
    [SerializeField] private float backgroundAlphaFactor = 0.7f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        yield return null;

        ConfigureStatsPanel();
        ConfigureInventoryPanel();
        ConfigureItemInfoPanel();
    }

    private void ConfigureStatsPanel()
    {
        float width = Screen.width * desiredWidthRation / statsPanel.lossyScale.x;
        statsPanel.offsetMax = statsPanel.offsetMax.With(x: width);

        statsPanelOpenedPos = statsPanel.anchoredPosition;
        statsPanelClosedPos = statsPanelOpenedPos + Vector2.left * width;

        statsPanel.anchoredPosition = statsPanelClosedPos;

        HidePlayerStats();
    }

    private void ConfigureInventoryPanel()
    {
        float width = Screen.width * desiredWidthRation / inventoryPanel.lossyScale.x;
        inventoryPanel.offsetMin = inventoryPanel.offsetMin.With(x: -width);

        inventoryPanelOpenedPos = inventoryPanel.anchoredPosition;
        inventoryPanelClosedPos = inventoryPanelOpenedPos + Vector2.right * width;

        inventoryPanel.anchoredPosition = inventoryPanelClosedPos;

        HideInventory(false);
    }

    private void ConfigureItemInfoPanel()
    {
        float height = Screen.height * desiredHeightRation / itemInfoPanel.lossyScale.y;
        itemInfoPanel.offsetMax = itemInfoPanel.offsetMax.With(y: height);

        itemInfoOpenedPos = itemInfoPanel.anchoredPosition;
        itemInfoClosedPos = itemInfoOpenedPos + Vector2.down * height;

        itemInfoPanel.anchoredPosition = itemInfoClosedPos;

        HideItemInfo();
    }

    public void ShowPlayerStats() => ShowPanel(statsPanel, statsBackground, statsPanelOpenedPos);
    public void HidePlayerStats() => HidePanel(statsPanel, statsBackground, statsPanelClosedPos);

    public void ShowInventory() => ShowPanel(inventoryPanel, inventoryBackground, inventoryPanelOpenedPos);
    public void HideInventory(bool hideItemInfo = true)
    {
        if (hideItemInfo)
            HideItemInfo();

        HidePanel(inventoryPanel, inventoryBackground, inventoryPanelClosedPos);    
    }

    public void ShowItemInfo() => ShowPanel(itemInfoPanel, null, itemInfoOpenedPos);
    public void HideItemInfo() => HidePanel(itemInfoPanel, null, itemInfoClosedPos);     

    public void ShowPanel(RectTransform panel, RectTransform background, Vector2 openPosition)
    {
        panel.gameObject.SetActive(true);

        LeanTween.cancel(panel);
        LeanTween.move(panel, openPosition, slideTime)
            .setEase(LeanTweenType.easeInCubic)
            .setIgnoreTimeScale(true);

        if (background != null)
        {
            background.gameObject.SetActive(true);
            background.GetComponent<Image>().raycastTarget = true;

            LeanTween.cancel(background);
            LeanTween.alpha(background, backgroundAlphaFactor, slideTime)
                .setRecursive(false)
                .setIgnoreTimeScale(true);
        }
    }

    public void HidePanel(RectTransform panel, RectTransform background, Vector2 closedPosition)
    {
        LeanTween.cancel(panel);
        LeanTween.move(panel, closedPosition, slideTime)
            .setEase(LeanTweenType.easeOutCubic)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => panel.gameObject.SetActive(false));

        if (background != null)
        {
            statsBackground.GetComponent<Image>().raycastTarget = false;

            LeanTween.cancel(background);
            LeanTween.alpha(background, 0, slideTime)
                .setRecursive(false)
                .setIgnoreTimeScale(true)
                .setOnComplete(() => background.gameObject.SetActive(false));      
        }
    }
}
