using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool IsSFXOn { get; private set; } = true;
    public bool IsMusicOn { get; private set; } = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        SettingsManager.onSFXStateChanged   += SetSFXStatus;
        SettingsManager.onMusicStateChanged += SetMusicStatus;
    }

    private void OnDisable()
    {
        SettingsManager.onMusicStateChanged -= SetSFXStatus;
        SettingsManager.onMusicStateChanged -= SetMusicStatus;
    }

    private void SetSFXStatus(bool status)   => IsSFXOn = status;
    private void SetMusicStatus(bool status) => IsMusicOn = status;
}
