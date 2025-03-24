using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel), typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public static Player instance = null;

    [Header("Components")]
    private PlayerHealth playerHealth;
    private PlayerLevel playerLevel;
    private CapsuleCollider2D cd;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
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
