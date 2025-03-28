using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel), typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public static Player instance = null;

    [Header("Components")]
    private PlayerHealth health;
    private PlayerDamageHandler damageHandler;
    private PlayerLevel playerLevel;
    private CapsuleCollider2D cd;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        health = GetComponent<PlayerHealth>();
        damageHandler = GetComponent<PlayerDamageHandler>();
        playerLevel = GetComponent<PlayerLevel>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    public void TakeDamage(int damage)
    {
        int realDamage = damageHandler.CalculateDamage(damage);

        if (realDamage != 0)
            health.ApplyDamage(realDamage);
    }
    
    public Vector2 GetCenterPoint()
    {
        return (Vector2)transform.position + new Vector2(0, cd.offset.y + cd.size.y / 2);
    }
}
