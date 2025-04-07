using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [Header("Currency")]
    [field: SerializeField] public int Currency { get; private set; }
    private CurrencyText[] currencyTexts;

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
    }

    private void UpdateTexts()
    {
        foreach (CurrencyText text in currencyTexts)
            text.UpdateText(Currency.ToString());
    }

    private void DonutCoinCollectionCallback(DonutCoin coin) => AddCurrency(1);
}