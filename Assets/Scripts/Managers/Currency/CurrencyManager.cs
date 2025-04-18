using System;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    Normal,
    Premium
}

[System.Serializable]
public class CurrencyData
{
    public CurrencyType Type;
    public int Amount;
}

public class CurrencyManager : MonoBehaviour, ISaveAndLoad
{
    public static CurrencyManager instance;
    private const string premiumCurrency = "PremiumCurrency";

    [SerializeField] List<CurrencyData> currencyDatas;
    private Dictionary<CurrencyType, CurrencyTextsHandler> handlers = new();

    public static Action onUpdated;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
            handlers[type] = new CurrencyTextsHandler(type);
    }

    private void Start()
    {          
        CurrencyText[] allTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (CurrencyText text in allTexts)
            handlers[text.CurrencyType].RegisterText(text);

        foreach (CurrencyData data in currencyDatas)
            AddCurrency(data.Type, data.Amount);
    }

    public bool HasEnoughCurrency(CurrencyType type, int amount) => handlers[type].HasEnough(amount);
    
    public void AddCurrency(CurrencyType type, int amount) => UpdateCurrency(type, amount, true);
    public void UseCurrency(CurrencyType type, int amount) => UpdateCurrency(type, amount, false);

    private void UpdateCurrency(CurrencyType type, int amount, bool isAdd)
    {
        if (isAdd)
            handlers[type].Add(amount);
        else
            handlers[type].Use(amount);
        
        onUpdated?.Invoke();

        if (type == CurrencyType.Premium)
            Save();        
    }

    public void Load()
    {
        CurrencyData premiumCurrencyData = null;
        
        for (int i = 0; i < currencyDatas.Count; i++)
        {
            if (currencyDatas[i].Type == CurrencyType.Premium)
            {
                premiumCurrencyData = currencyDatas[i];
                break;
            }
        }

        if (premiumCurrencyData == null)
            return;

        if (SaveManager.TryLoad(this, premiumCurrency, out object premiumCurrencyValue))
            premiumCurrencyData.Amount = (int)premiumCurrencyValue;
        else
            premiumCurrencyData.Amount = 100;
    }

    public void Save()
    {
        int amount = handlers[CurrencyType.Premium].Amount;
        SaveManager.Save(this, premiumCurrency, amount);
    }
}