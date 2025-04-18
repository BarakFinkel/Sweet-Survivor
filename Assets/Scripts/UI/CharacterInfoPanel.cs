using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image currencyIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject priceContainer;
    [SerializeField] private Transform statsParent;
    
    [Header("Button Colors")]
    [SerializeField] private Color interactableColor;
    [SerializeField] private Color uninteractableColor;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(CharacterDataSO characterData, bool unlocked)
    {
        nameText.text = characterData.Name;
        StatContainerManager.GenerateStatContainers(characterData.NonNeutralStats, statsParent);

        if (unlocked)
        {
            SetButtonState
            (
                false,
                false,
                uninteractableColor,
                TextAlignmentOptions.Center,
                "UNLOCKED!"
            );
        }
        else if (!CurrencyManager.instance.HasEnoughCurrency(CurrencyType.Premium, characterData.PurchasePrice))
        {
            SetButtonState
            (
                false,
                true,
                uninteractableColor,
                TextAlignmentOptions.Right,
                characterData.PurchasePrice.ToString());
        }
        else
        {
            SetButtonState
            (
                true,
                true,
                interactableColor,
                TextAlignmentOptions.Right,
                characterData.PurchasePrice.ToString()
            );
        }
    }

    private void SetButtonState(bool interactable, bool showCurrencyIcon, Color bgColor, TextAlignmentOptions alignment, string priceTextValue)
    {
        Button.interactable = interactable;
        currencyIcon.gameObject.SetActive(showCurrencyIcon);
        buttonImage.color = bgColor;
        priceText.alignment = alignment;
        priceText.text = priceTextValue;
    }
}
