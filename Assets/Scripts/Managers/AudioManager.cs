using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

using Random = UnityEngine.Random;

[Serializable]
public struct MusicData
{
    public GameState type;
    public List<AudioClip> clips;
}

[Serializable]
public struct GameStateSFXData
{
    public GameState type;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool IsSFXOn { get; private set; } = true;
    
    [Header("UI SFX Settings")]
    [SerializeField] private AudioSource uiSFXSource;
    [SerializeField] private AudioClip buttonSound;

    [Header("Game State SFX Settings")]
    [SerializeField] private AudioSource gameStateSFXSource;
    [SerializeField] private List<GameStateSFXData> gameStateSFXDatas;
    private Dictionary<GameState, AudioClip> gameStateSFXDictionary;

    //////////////////////////////////////////////////////////////////

    public bool IsMusicOn { get; private set; } = true;

    [Header("Music Settings")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<MusicData> musicDatas;
    private Dictionary<GameState, List<AudioClip>> musicDictionary;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InitializeMusicDictionary();
        InitializeSFXDictionary();

        GameManager.onGameStateChanged      += GameStateChangedCallback;
        SettingsManager.onSFXStateChanged   += SetSFXStatus;
        SettingsManager.onMusicStateChanged += SetMusicStatus;
    }

    private void Start()
    {
        IsSFXOn   = SettingsManager.instance.sfxOn;
        IsMusicOn = SettingsManager.instance.musicOn;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged      -= GameStateChangedCallback;
        SettingsManager.onSFXStateChanged   -= SetSFXStatus;
        SettingsManager.onMusicStateChanged -= SetMusicStatus;
    }

    # region Music

    private void SetMusicStatus(bool status)
    {
        IsMusicOn = status;

        if (IsMusicOn)
        {
            musicSource.clip = musicDictionary[GameState.MENU][0];
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }
    }

    private void InitializeMusicDictionary()
    {
        musicDictionary = new Dictionary<GameState, List<AudioClip>>();

        foreach (MusicData data in musicDatas)
        {
            if (!musicDictionary.ContainsKey(data.type))
                musicDictionary[data.type] = new List<AudioClip>();

            musicDictionary[data.type].AddRange(data.clips);
        }
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        ChangeMusic(gameState);
        PlayGameStateSFX(gameState);
    }

    private void ChangeMusic(GameState gameState)
    {
        if (gameState == GameState.GAMEOVER)
        {
            musicSource.Stop();
            return;
        }

        // If we have music corresponding to the new gamestate, we go in.
        if (IsMusicOn && musicDictionary.ContainsKey(gameState))
        {
            // Choose a random clip from the corresponding list.
            List<AudioClip> musicClips = musicDictionary[gameState];
            AudioClip randomClip = musicClips[Random.Range(0, musicClips.Count)];

            // If we aren't playing the clip already, 
            if (musicSource.clip != randomClip)
            {
                musicSource.clip = randomClip;
                musicSource.Play();
            }
        }
    }

    # endregion

    # region SFX

    private void SetSFXStatus(bool status) => IsSFXOn = status;

    private void InitializeSFXDictionary()
    {
        gameStateSFXDictionary = new Dictionary<GameState, AudioClip>();

        foreach (GameStateSFXData data in gameStateSFXDatas)
            gameStateSFXDictionary[data.type] = data.clip; 
    }

    private void PlayGameStateSFX(GameState gameState)
    {
        // If we have an sfx corresponding to the new gamestate, we play it.
        if (gameStateSFXDictionary.ContainsKey(gameState))
        {
            gameStateSFXSource.clip = gameStateSFXDictionary[gameState];
            gameStateSFXSource.Play();
        }
    }

    public void PlayButtonSound()
    {
        if (IsSFXOn)
        {
            uiSFXSource.clip = buttonSound;
            uiSFXSource.Play();
        }
    }

    #endregion
}
