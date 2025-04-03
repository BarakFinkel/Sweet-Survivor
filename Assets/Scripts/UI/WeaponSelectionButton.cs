using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using Unity.Collections;

public class WeaponSelectionButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Transform statContainersParent;

    [Header("Settings")]
    private int weaponLevel;

    [Header("Color Settings")]
    [SerializeField] private Color selectButtonColor;
    [SerializeField] private float colorLerpTime = 1.0f;

    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public Image ButtonImage { get; private set; }

    private float lerpProgression = 0.0f;
    private bool selected = false;

    public void Configure(Sprite _sprite, string _name, int _level, WeaponDataSO _weaponData)
    {
        weaponIcon.sprite = _sprite;
        weaponName.text = _name;
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
            Debug.Log("Entered!");
        }
        else if (!selected && lerpProgression > 0.0f)
        {
            lerpProgression = Mathf.Max(lerpProgression - Time.unscaledDeltaTime, 0.0f);
        }

        ButtonImage.color = Color.Lerp(Color.white, selectButtonColor, lerpProgression / colorLerpTime);
    }

    public int GetWeaponLevel()
    {
        return weaponLevel;
    }

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }
}
