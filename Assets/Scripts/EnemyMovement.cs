using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player;
    
    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    void Start()
    {
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found, destroying enemy.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)(transform.position) + direction * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }
}
