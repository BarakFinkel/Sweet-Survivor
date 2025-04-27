using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeValueText;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite _icon, string _upgradeName, string _upgradeValue)
    {
        image.sprite = _icon;
        upgradeNameText.text = _upgradeName;
        upgradeValueText.text = _upgradeValue;
    }
}
