using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [Header("Currency")]
    [field: SerializeField] public int Currency { get; private set; }
    private CurrencyText[] currencyTexts;

    [Header("Actions")]
    public static Action onUpdated;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DonutCoin.onCollect += DonutCoinCollectionCallback;
    }

    private void Start()
    {
        currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        UpdateTexts();
    }

    public void OnDisable()
    {
        DonutCoin.onCollect -= DonutCoinCollectionCallback;
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateTexts();

        onUpdated?.Invoke();
    }

    public void UseCurrency(int price) => AddCurrency(-price);

    public bool HasEnoughCurrency(int price) => Currency >= price;

    private void UpdateTexts()
    {
        foreach (CurrencyText text in currencyTexts)
            text.UpdateText(Currency.ToString());
    }

    private void DonutCoinCollectionCallback(DonutCoin coin) => AddCurrency(1);
}