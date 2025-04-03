using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image statIcon;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    public void Configure(Sprite _icon, string _statName, string _statValue)
    {
        statIcon.sprite = _icon;
        statNameText.text = _statName;
        statValueText.text = _statValue;
    }

    public float GetFontSize()
    {
        return statNameText.fontSize;
    }

    public void SetFontSize(float size)
    {
        statNameText.fontSizeMax  = size;
        statValueText.fontSizeMax = size;
    }
}
