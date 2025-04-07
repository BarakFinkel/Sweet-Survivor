using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectionButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Transform statContainersParent;
    [SerializeField] private Outline outline;

    [Header("Color Settings")]
    [SerializeField] private Color selectButtonColor;
    [SerializeField] private float colorLerpTime = 1.0f;

    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public Image ButtonImage { get; private set; }

    private float lerpProgression = 0.0f;
    private bool selected = false;

    public void Configure(WeaponDataSO _weaponData, int _level)
    {
        weaponIcon.sprite = _weaponData.Sprite;
        weaponName.text = _weaponData.Name;
        weaponName.color = ColorHolder.GetLevelColor(_level);

        Dictionary<Stat, float> weaponStats = WeaponStatsCalculator.GetStats(_weaponData, _level);

        ConfigureStatsContainers(weaponStats);
    }

    private void ConfigureStatsContainers(Dictionary<Stat, float> weaponStats)
    {
        StatContainerManager.GenerateStatContainers(weaponStats, statContainersParent);
    }

    public void Update()
    {
        if (selected && lerpProgression < colorLerpTime)
        {
            lerpProgression = Mathf.Min(lerpProgression + Time.unscaledDeltaTime, colorLerpTime);
        }
        else if (!selected && lerpProgression > 0.0f)
        {
            lerpProgression = Mathf.Max(lerpProgression - Time.unscaledDeltaTime, 0.0f);
        }

        ButtonImage.color = Color.Lerp(Color.white, selectButtonColor, lerpProgression / colorLerpTime);
    }

    public void Select()
    {
        selected = true;
        outline.enabled = true;
    }

    public void Deselect()
    {
        selected = false;
        outline.enabled = false;
    }
}
