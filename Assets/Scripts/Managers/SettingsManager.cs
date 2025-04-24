using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;

public class SettingsManager : MonoBehaviour, ISaveAndLoad
{
    public static SettingsManager instance;

    [Header("Elements")]
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button supportButton;
    [SerializeField] private Button privacyPolicyButton;
    [SerializeField] private Button creditsButton;

    [Header("Settings")]
    [SerializeField] private Color onColor;
    [SerializeField] private Material onMaterial;
    [SerializeField] private Color offColor;
    [SerializeField] private Material offMaterial;

    [Header("Data")]
    private const string sfxKey   = "SFX";
    private const string musicKey = "Music";
    public bool sfxOn { get; private set; } = true;
    public bool musicOn { get; private set; } = true;

    [Header("Action")]
    public static Action<bool> onSFXStateChanged;
    public static Action<bool> onMusicStateChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);   
    }

    public void SFXButtonCallback()
    {
        sfxOn = !sfxOn;
        
        UpdateSoundButtonVisuals(sfxButton, sfxOn);
        onSFXStateChanged?.Invoke(sfxOn);

        Save();
    }

    public void MusicButtonCallback()
    {
        musicOn = !musicOn;
        
        UpdateSoundButtonVisuals(musicButton, musicOn);
        onMusicStateChanged?.Invoke(musicOn);

        Save();
    }

    public void SupportButtonCallback()
    {
        string email = "lightninggamedev@gmail.com";
        string subject = "Help";
        string body = "Hey, I really need help with this...";

        Application.OpenURL("mailto:" + email + "?subject=" + CreateEscapeURL(subject) + "&body=" + CreateEscapeURL(body));
    }

    public void PrivacyPolicyButtonCallback()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }

    public void CreditsButtonCallback()
    {
        UIManager.instance.ShowCreditsPanel();
    }

    private void UpdateSoundButtonsVisuals()
    {
        UpdateSoundButtonVisuals(sfxButton, sfxOn);
        UpdateSoundButtonVisuals(musicButton, musicOn);
    }

    private void UpdateSoundButtonVisuals(Button button, bool status)
    {
        if (status)
        {
            button.image.color = onColor;
            TextMeshProUGUI tmp = button.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = "ON";
            tmp.fontMaterial = onMaterial;
        }
        else
        {
            button.image.color = offColor;
            TextMeshProUGUI tmp = button.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = "Off";
            tmp.fontMaterial = offMaterial;
        }
    }

    private string CreateEscapeURL(string s)
    {
        return UnityWebRequest.EscapeURL(s).Replace("+", "%20");
    }

    public void Load()
    {
        sfxOn = true;
        musicOn = true;

        if (SaveManager.TryLoad(this, sfxKey, out object sfxStateObject))
            sfxOn = (bool)sfxStateObject;

        if (SaveManager.TryLoad(this, musicKey, out object musicStateObject))
            musicOn = (bool)musicStateObject;

        UpdateSoundButtonsVisuals();
    }

    public void Save()
    {
        SaveManager.Save(this, sfxKey, sfxOn);
        SaveManager.Save(this, musicKey, musicOn);
    }
}
