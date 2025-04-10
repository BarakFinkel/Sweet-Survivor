using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

/// <summary>
/// An enum representing types of different game states.
/// </summary>
public enum GameState
{
    MENU,
    WEAPONSELECT,
    GAME,
    LEVELUP,
    CHESTOPEN,
    SHOP,
    STAGECOMPLETE,
    GAMEOVER
}

/// <summary>
/// Manages the overall game state and notifies listeners when it changes.
/// Implements a singleton pattern to ensure only one instance exists.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Action<GameState> onGameStateChanged;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU);
    }

    /// <summary>
    /// Changes the game state to Game.
    /// </summary>
    public void StartGame() => SetGameState(GameState.GAME);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECT);
    public void ManageGameOver() => SceneManager.LoadScene(0);

    /// <summary>
    /// Changes the game state to the desired one within a delay.
    /// </summary>
    /// <param name="nextGameState">The new game state to set.</param>
    /// <param name="delay">The delay length before setting the new game state.</param>
    public void WaveCompleteCallback(GameState nextGameState, float delay)
    {
        StartCoroutine(SetGameStateWithDelay(nextGameState, delay));
    }

    /// <summary>
    /// Calls the SetGameState() method after the given delay.
    /// </summary>
    /// <param name="gameState">The new game state to set.</param>
    /// <param name="delay">The delay length before setting the new game state.</param>
    private IEnumerator SetGameStateWithDelay(GameState gameState, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetGameState(gameState);
    }

    /// <summary>
    /// Updates the game state and notifies all registered listeners.
    /// </summary>
    /// <param name="gameState">The new game state to set.</param>
    public void SetGameState(GameState gameState)
    {
        if (gameState == GameState.GAME)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }

        onGameStateChanged?.Invoke(gameState);
    }
}
