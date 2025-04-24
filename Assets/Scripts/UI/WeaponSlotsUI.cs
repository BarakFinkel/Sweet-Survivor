using UnityEngine;
using TMPro;

public class WeaponSlotsUI : MonoBehaviour
{
    [Header("Elements")]
    private TextMeshProUGUI weaponSlotText;

    private void Awake()
    {
        weaponSlotText = GetComponent<TextMeshProUGUI>();
        PlayerWeaponsManager.onWeaponsChanged += WeaponsChangedCallback;
    }

    private void OnEnable()
    {
        UpdateText();
    }

    private void OnDestroy()
    {
        PlayerWeaponsManager.onWeaponsChanged -= WeaponsChangedCallback;
    }

    private void WeaponsChangedCallback() => UpdateText();

    private void UpdateText()
    {
        weaponSlotText.text = PlayerWeaponsManager.instance.GetNumberOfWeapons() + "/6";
    }
}
