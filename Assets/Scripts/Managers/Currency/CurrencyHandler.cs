using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds and updates a list of currency texts of a given type.
/// </summary>
public class CurrencyTextsHandler
{
    public CurrencyType Type { get; private set; }
    public int Amount { get; private set; }

    private List<CurrencyText> linkedTexts;

    /// <summary>
    /// Constructor method for the currency type.
    /// </summary>
    /// <param name="type">The type of currency this handler manages.</param>
    public CurrencyTextsHandler(CurrencyType _type)
    {
        Type = _type;
        linkedTexts = new List<CurrencyText>();
    }

    public void RegisterText(CurrencyText text) => linkedTexts.Add(text);

    public bool HasEnough(int amount) => Amount >= amount;

    public void Use(int amount) => Add(-amount);

    public void Add(int amount)
    {
        Amount += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        string currentAmount = Amount.ToString();

        foreach (var text in linkedTexts)
            text.UpdateText(currentAmount);
    }
}
