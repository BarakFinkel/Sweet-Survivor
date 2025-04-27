using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private Image containerImage;
    [SerializeField] private Button button;
    [SerializeField] public Outline outline;

    [Header("Container Outline Settings")]
    public static InventoryItemContainer lastSelectedContainer;
    public static Action onContainerSelected;

    [Header("Item Contained Settings")]
    public Weapon Weapon { get; private set; }
    public int WeaponIndex { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }

    private void OnEnable() => onContainerSelected += ContainerSelectedCallback;
    private void OnDisable() => onContainerSelected -= ContainerSelectedCallback;

    public void Configure(Weapon _weapon, int _index, Action _clickedCallback)
    {
        Weapon = _weapon;
        WeaponIndex = _index;

        ConfigureVisuals(ColorHolder.GetLevelColor(_weapon.level), _weapon.WeaponData.Icon);
        ConfigureButton(_clickedCallback);
    }

    public void Configure(ObjectDataSO _objectData, Action _clickedCallback)
    {
        ObjectData = _objectData;
        
        ConfigureVisuals(ColorHolder.GetLevelColor(_objectData.Rarity), _objectData.Icon);
        ConfigureButton(_clickedCallback);
    }

    public void ConfigureVisuals(Color _color, Sprite _icon)
    {
        containerImage.color = _color;
        icon.sprite          = _icon;
    }

    public void ConfigureButton(Action _clickedCallback)
    {
        button.onClick.RemoveAllListeners();
        
        if (_clickedCallback != null)
        {
            button.interactable = true;
            button.onClick.AddListener(SelectThisContainer);
            button.onClick.AddListener(() => _clickedCallback?.Invoke());
            button.onClick.AddListener(AudioManager.instance.PlayButtonSound);
        }
        else
        {
            button.interactable = false;
        }
    }

    public void SelectThisContainer()
    {
        lastSelectedContainer = this;
        onContainerSelected?.Invoke();
    }

    public static void DeselectHighlightedContainer()
    {
        if (lastSelectedContainer != null)
        {
            lastSelectedContainer.outline.enabled = false;
            lastSelectedContainer = null;
        }
    }

    private void ContainerSelectedCallback()
    {
        if (lastSelectedContainer != null && lastSelectedContainer == this)
            outline.enabled = true;
        else
            outline.enabled = false;
    }
}