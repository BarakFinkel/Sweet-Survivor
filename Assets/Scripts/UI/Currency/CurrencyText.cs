using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurrencyText : MonoBehaviour
{
    [field:SerializeField] public CurrencyType CurrencyType { get; private set; }
    protected TextMeshProUGUI Text => GetComponent<TextMeshProUGUI>();

    public virtual void UpdateText(string value) => Text.text = value;
}