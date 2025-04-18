using UnityEngine;

public class HapticFeedback : MonoBehaviour
{
    private void Awake()
    {
        PlayerHealth.onDamage += VibrateMedium;
        Player.onDeath += VibrateHard;
    }

    private void OnDisable()
    {
        PlayerHealth.onDamage -= VibrateMedium;
        Player.onDeath -= VibrateHard;
    }

    private void VibrateMedium()
    {
        CandyCoded.HapticFeedback.HapticFeedback.MediumFeedback();
    }

    private void VibrateHard()
    {
        CandyCoded.HapticFeedback.HapticFeedback.HeavyFeedback();
    }
}
