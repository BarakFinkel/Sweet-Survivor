using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    private PlayerHealth playerHealth;
    private CapsuleCollider2D cd;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }
    
    public Vector2 GetCenterPoint()
    {
        return (Vector2)transform.position + new Vector2(0, cd.offset.y + cd.size.y / 2);
    }
}
