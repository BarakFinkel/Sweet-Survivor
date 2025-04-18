using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CharacterButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject lockOverlay;
    [SerializeField] private RectMask2D mask;

    public Button Button 
    { 
        get 
        {
            return GetComponent<Button>();
        }

        private set
        {} 
    }

    public void Configure(Sprite _icon, bool unlocked)
    {
        characterImage.sprite = _icon;

        if (unlocked)
            Unlock();
        else
            Lock();
    }

    public void Lock()
    {
        lockOverlay.SetActive(true);
        mask.enabled = true;       
    }

    public void Unlock()
    {
        lockOverlay.SetActive(false);
        mask.enabled = false;
    }
}
