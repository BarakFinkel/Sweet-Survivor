using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Bounds", menuName = "Scriptable Objects/New Map Bounds Data", order = 0)]
public class MapBoundsSO : ScriptableObject
{
    [field: SerializeField] public Vector2 bounds { get; private set; }
}
