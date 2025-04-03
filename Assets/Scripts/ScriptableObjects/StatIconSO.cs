using UnityEngine;

[CreateAssetMenu(fileName = "Stat Icons", menuName = "Scriptable Objects/Stat Icons Data", order = 0)]
public class StatIconsSO : ScriptableObject
{
    [field: SerializeField] public StatIcon[] StatIcons { get; private set; }
}

[System.Serializable]
public struct StatIcon
{
    public Stat stat;
    public Sprite icon;
}