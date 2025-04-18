using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CreditsScroller : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private float scrollSpeed;
    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();   
    }

    private void OnEnable()
    {
        rt.anchoredPosition = rt.anchoredPosition.With(y: 0);
    }

    private void Update()
    {
        rt.anchoredPosition += Vector2.up * Time.unscaledDeltaTime * scrollSpeed;
    }
}
