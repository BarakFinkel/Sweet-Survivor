using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image statIcon;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;
    
    [Header("Text Colors")]
    [SerializeField] Color neutralColor;
    [SerializeField] Color positiveColor;
    [SerializeField] Color negativeColor;

    public void Configure(Sprite _icon, string _statName, float _statValue, bool useColor = false)
    {
        statIcon.sprite = _icon;
        statNameText.text = _statName;

        if (useColor)
        {
            ColorStatValues(_statValue);
        }
        else
        {
            statValueText.color = neutralColor;
            statValueText.text = _statValue.ToString("F1");
        }
    }

    private void ColorStatValues(float statValue)
    {
        // Determine the value's sign
        float sign = Mathf.Sign(statValue);
        if (statValue == 0)
            sign = 0;

        // Set the color of the text based on the sign.
        Color statValueColor = Color.white;

        if (sign > 0)
            statValueColor = positiveColor;
        else if (sign < 0)
            statValueColor = negativeColor;

        // Set the text's fields accordingly.
        statValueText.color = statValueColor;
        statValueText.text = statValue.ToString("F1");
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
