using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurrencyText : MonoBehaviour
{
    [Header("Elements")]
    private TextMeshProUGUI currencyText => GetComponent<TextMeshProUGUI>();

    public void UpdateText(string _currencyText) => currencyText.text = _currencyText;
}
