using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Player player;
    private WaveManagerUI ui;
    
    [Header("General Settings")]
    [SerializeField] private float waveDuration;
    private float timer = 0;
    private bool isTimerOn;

    [Header("Spawn Settings")]
    [SerializeField] private float minOffset = 8.0f;
    [SerializeField] private float maxOffset = 18.0f;
    [SerializeField] private float xBound = 41.0f;
    [SerializeField] private float yBound = 23.0f;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    private int currentWaveIndex = 0;
    private List<float> segmentSpawnCounters = new List<float>();

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }

    private void Start()
    {
        ResetSegmentCounters();
        StartWave();
    }

    private void Update()
    {
        if(!isTimerOn)
            return;

        if (timer < waveDuration)
        {
            ManageCurrentWave();

            string timerString = Mathf.Max(waveDuration - timer, 0).ToString("F2");
            ui.UpdateWaveTimer(timerString);
        }
        else if (transform.childCount == 0)
        {
           ui.UpdateWaveTimer(0.0f.ToString("F2"));
            StartWaveTransition();
        }

        timer += Time.deltaTime;
    }

    private void StartWave()
    {
        ui.UpdateWaveText("Wave " + (currentWaveIndex + 1) + " / " + waves.Length);
        ui.UpdateWaveTimer(waveDuration.ToString("F2"));

        ResetSegmentCounters();
        timer = 0.0f;
        isTimerOn = true;
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;
        currentWaveIndex++;
        
        if (currentWaveIndex < waves.Length) 
            StartWave();
        else
        {
            ui.UpdateWaveText("Stage Complete!");
            ui.UpdateWaveTimer("");
        }
    }

    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        // We go over all segments within the current wave
        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment currentSegment = currentWave.segments[i];

            float tStart = currentSegment.t0 * waveDuration;
            float tEnd = currentSegment.t1 * waveDuration;

            // If we are within the current segment's interval, we move in.
            if (tStart <= timer && timer <= tEnd)
            {         
                Debug.Log("Entered!");

                float delta = timer - tStart;
                float delay = 1.0f / currentSegment.spawnFrequency;

                // According to the frequency - if it's time to spawn another prefab, we do so and increment the segment's matching counter.
                if (delta / delay > segmentSpawnCounters[i])
                {
                    
                    Instantiate(currentSegment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                    segmentSpawnCounters[i]++;
                }
            }
        }
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere.normalized;
        Vector2 offset = direction * Random.Range(minOffset, maxOffset);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -xBound, xBound);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -yBound, yBound);

        return targetPosition;
    }

    private void ResetSegmentCounters()
    {
        segmentSpawnCounters.Clear();
        
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0 ; i < waves[currentWaveIndex].segments.Count; i++)
            {
                segmentSpawnCounters.Add(1);
            }
        }
    }
}

#region Wave & Segment Structs

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    // Relative time stamps from 0.0 to 1.0 to declare in which segment of the wave the prefab will spawn.
    [Range(0, 1)] public float t0;
    [Range(0, 1)] public float t1;
    
    // The frequency (time per second) in which the prefab will spawn.
    public float spawnFrequency;

    // The type of prefab to be spawned.
    public GameObject prefab;
}

#endregion