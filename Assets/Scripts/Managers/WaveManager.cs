using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages enemy wave spawning and transitions between waves.
/// Implements <see cref="IGameStateListener"/> to react to game state changes.
/// </summary>
[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    /// <summary>
    /// Reference to the player object.
    /// </summary>
    [SerializeField] private Player player;

    /// <summary>
    /// UI manager for displaying wave-related information.
    /// </summary>
    private WaveManagerUI ui;
    
    [Header("General Settings")]
    /// <summary>
    /// Duration of each wave in seconds.
    /// </summary>
    [SerializeField] private float waveDuration;

    /// <summary>
    /// Timer tracking the current wave's elapsed time.
    /// </summary>
    private float timer = 0;

    /// <summary>
    /// Flag to ensure the UI timer reaches zero exactly once per wave.
    /// </summary>
    private bool changedTimerTextToZero = false;

    /// <summary>
    /// Flag to indicate wave completions.
    /// </summary>
    private bool waveComplete = true;

    /// <summary>
    /// Indicates whether the wave timer is active.
    /// </summary>
    private bool isTimerOn;

    [Header("Spawn Settings")]
    /// <summary>
    /// Minimum spawn offset from the player.
    /// </summary>
    [SerializeField] private float minOffset = 8.0f;

    /// <summary>
    /// Maximum spawn offset from the player.
    /// </summary>
    [SerializeField] private float maxOffset = 18.0f;

    /// <summary>
    /// Horizontal boundary for enemy spawn positions.
    /// </summary>
    [SerializeField] private float xBound = 41.0f;

    /// <summary>
    /// Vertical boundary for enemy spawn positions.
    /// </summary>
    [SerializeField] private float yBound = 23.0f;

    [Header("Waves")]
    /// <summary>
    /// Array of waves containing enemy spawn configurations.
    /// </summary>
    [SerializeField] private Wave[] waves;

    /// <summary>
    /// Index of the current wave.
    /// </summary>
    private int currentWaveIndex = 0;

    /// <summary>
    /// Tracks the spawn progress of each segment in the current wave.
    /// </summary>
    private List<float> segmentSpawnCounters = new List<float>();

    /// <summary>
    /// Initializes references to necessary components.
    /// </summary>
    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }

    /// <summary>
    /// Starts the first wave when the game begins.
    /// </summary>
    private void Start()
    {
        ResetSegmentCounters();
    }

    /// <summary>
    /// Manages wave progression and spawning over time.
    /// </summary>
    private void Update()
    {
        if (!isTimerOn)
            return;

        // If we haven't reached the end of the wave duration, we handle the wave and update the timer.
        if (timer < waveDuration)
        {
            ManageCurrentWave();
            
            string timerString = Mathf.Max(waveDuration - timer, 0).ToString("F1");
            ui.UpdateWaveTimer(timerString);
        }
        else // Otherwise, we have 2 cases:
        {
            // If the timer just changed to 0, we update 
            if (!changedTimerTextToZero)
            {
                changedTimerTextToZero = true;
                ui.UpdateWaveTimer(0.0f.ToString("F1"));
            }

            if (transform.childCount == 0)
            {
                changedTimerTextToZero = false;
                StartWaveTransition();
            }
        }

        timer += Time.deltaTime;
    }

    /// <summary>
    /// Starts a new wave, updating the UI and resetting relevant parameters.
    /// </summary>
    private void StartWave()
    {
        ui.UpdateWaveText($"Wave {currentWaveIndex + 1} / {waves.Length}");
        ui.UpdateWaveTimer(waveDuration.ToString("F2"));

        ResetSegmentCounters();
        timer = 0.0f;
        isTimerOn = true;
        waveComplete = false;
    }

    /// <summary>
    /// Handles the transition between waves, progressing to the next wave if available.
    /// </summary>
    private void StartWaveTransition()
    {
        isTimerOn = false;
        currentWaveIndex++;
        waveComplete = true;

        if (currentWaveIndex < waves.Length)
        {
            GameManager.instance.WaveCompleteCallback(GameState.SHOP, 1.0f);
        }

        else
        {
            ui.UpdateWaveText("Stage Complete!");
            ui.UpdateWaveTimer("");
            
            GameManager.instance.WaveCompleteCallback(GameState.STAGECOMPLETE, 1.0f);
        }
    }

    /// <summary>
    /// Manages enemy spawning for the current wave based on time intervals.
    /// </summary>
    private void ManageCurrentWave()
    {
        
        Wave currentWave = waves[currentWaveIndex];

        // Iterate over all segments within the current wave.
        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment currentSegment = currentWave.segments[i];
            float tStart = currentSegment.t0 * waveDuration;
            float tEnd = currentSegment.t1 * waveDuration;

            // If the current time falls within this segment’s interval, handle spawning.
            if (tStart <= timer && timer <= tEnd)
            {
                float delta = timer - tStart;
                float delay = 1.0f / currentSegment.spawnFrequency;

                if (delta / delay > segmentSpawnCounters[i])
                {
                    Instantiate(currentSegment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                    segmentSpawnCounters[i]++;
                }
            }
        }
    }

    /// <summary>
    /// Generates a valid spawn position around the player within defined boundaries.
    /// </summary>
    /// <returns>A randomly offset position for enemy spawning.</returns>
    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere.normalized;
        Vector2 offset = direction * Random.Range(minOffset, maxOffset);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -xBound, xBound);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -yBound, yBound);

        return targetPosition;
    }

    /// <summary>
    /// Resets spawn counters for each segment in the current wave.
    /// </summary>
    private void ResetSegmentCounters()
    {
        segmentSpawnCounters.Clear();

        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].segments.Count; i++)
            {
                segmentSpawnCounters.Add(1);
            }
        }
    }

    /// <summary>
    /// Responds to changes in game state.
    /// </summary>
    /// <param name="gameState">The new game state.</param>
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                if (waveComplete)
                    StartWave();
                break;

            default:
                break;
        }
    }
}

#region Wave & Segment Structs

/// <summary>
/// Represents a wave containing multiple segments of enemy spawns.
/// </summary>
[System.Serializable]
public struct Wave
{
    /// <summary>
    /// Name of the wave.
    /// </summary>
    public string name;

    /// <summary>
    /// List of enemy spawn segments within this wave.
    /// </summary>
    public List<WaveSegment> segments;
}

/// <summary>
/// Defines a segment of a wave, specifying spawn frequency and timing.
/// </summary>
[System.Serializable]
public struct WaveSegment
{
    /// <summary>
    /// Prefab of the enemy or object to be spawned.
    /// </summary>
    public GameObject prefab;

    /// <summary>
    /// The spawn frequency (objects per second) for this segment.
    /// </summary>
    public float spawnFrequency;
    
    /// <summary>
    /// Relative start time (0.0 to 1.0) for this segment within the wave.
    /// </summary>
    [Range(0, 1)] public float t0;

    /// <summary>
    /// Relative end time (0.0 to 1.0) for this segment within the wave.
    /// </summary>
    [Range(0, 1)] public float t1;
}

#endregion
