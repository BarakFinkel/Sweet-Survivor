using UnityEngine;
using TMPro;
using System.Collections;

public class WeaponSlotsUI : MonoBehaviour
{
    [Header("Elements")]
    private TextMeshProUGUI weaponSlotText;

    private void Awake()
    {
        weaponSlotText = GetComponent<TextMeshProUGUI>();
        PlayerWeaponsManager.onWeaponsChanged += WeaponsChangedCallback;
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

    private IEnumerator UpdateTextWithDelay()
    {
        yield return null;

        weaponSlotText.text = PlayerWeaponsManager.instance.GetNumberOfWeapons() + "/6";
    }
}
