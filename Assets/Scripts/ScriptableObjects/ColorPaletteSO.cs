using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette", menuName = "Scriptable Objects/New Color Palette Data", order = 2)]
public class ColorPaletteSO : ScriptableObject
{
    [field: SerializeField] public Color[] innerColors   { get; private set; }
    [field: SerializeField] public Color[] outlineColors { get; private set; }
}
