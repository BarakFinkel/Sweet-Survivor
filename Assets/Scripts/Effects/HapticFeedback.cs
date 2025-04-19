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
#if UNITY_IOS || UNITY_ANDROID
        CandyCoded.HapticFeedback.HapticFeedback.MediumFeedback();
#endif
    }

    private void VibrateHard()
    {
#if UNITY_IOS || UNITY_ANDROID
        CandyCoded.HapticFeedback.HapticFeedback.HeavyFeedback();
#endif
    }
}
