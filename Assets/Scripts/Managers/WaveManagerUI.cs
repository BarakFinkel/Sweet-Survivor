using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;

    public void UpdateWaveText(string waveString) => waveText.text = waveString;
    public void UpdateWaveTimer(string timerString) => timerText.text = timerString;
    public void EnableTimerText()  => timerText.enabled = true;    
    public void DisableTimerText() => timerText.enabled = false;
}
