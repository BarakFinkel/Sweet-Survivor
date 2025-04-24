using UnityEngine;

/// <summary>
/// Used in order to store coordinates of the bounds coordinates within the game map.
/// </summary>
public class MapBoundsHolder : MonoBehaviour
{
    public static MapBoundsHolder instance;
    
    [Header("Elements")]
    [field: SerializeField] public MapBoundsSO BoundsData { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public float GetXBound() => BoundsData.bounds.x;
    public float GetYBound() => BoundsData.bounds.y;
}
