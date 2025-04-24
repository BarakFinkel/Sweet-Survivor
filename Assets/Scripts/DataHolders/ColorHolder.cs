using UnityEngine;

/// <summary>
/// Used in order to store color data.
/// </summary>
public class ColorHolder : MonoBehaviour
{
    public static ColorHolder instance;

    [Header("Elements")]
    [SerializeField] private ColorPaletteSO levelPalette;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static Color GetLevelColor(int level)
    {
        if (1 <= level && level <= instance.levelPalette.innerColors.Length)
        {
            return instance.levelPalette.innerColors[level - 1];
        }
        else
        {
            Debug.LogError("Error - level not valid.");
            return Color.white;
        }
    }

    public static Color GetLevelOutlineColor(int level)
    {
        if (1 <= level && level <= instance.levelPalette.outlineColors.Length)
        {
            return instance.levelPalette.outlineColors[level - 1];
        }
        else
        {
            Debug.LogError("Error - level not valid.");
            return Color.white;
        }
    }
}
