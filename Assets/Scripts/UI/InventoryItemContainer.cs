using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image containerImage;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public Weapon weapon { get; private set; }
    public int Index { get; private set; }

    public ObjectDataSO objectData { get; private set; }

    public void Configure(Weapon _weapon, int _index, Action _clickedCallback)
    {
        weapon = _weapon;
        Index = _index;

        ConfigureVisuals(ColorHolder.GetLevelColor(_weapon.level), _weapon.WeaponData.Icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => _clickedCallback?.Invoke());
    }

    public void Configure(ObjectDataSO _objectData, Action _clickedCallback)
    {
        objectData = _objectData;
        
        ConfigureVisuals(ColorHolder.GetLevelColor(_objectData.Rarity), _objectData.Icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => _clickedCallback?.Invoke());
    }
    
    public void ConfigureVisuals(Color _color, Sprite _icon)
    {
        containerImage.color = _color;
        icon.sprite          = _icon;
    }
}