using System;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel), typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Components")]
    [SerializeField] private SpriteRenderer playerRenderer;
    private PlayerHealth health;
    private PlayerDamageHandler damageHandler;
    private CapsuleCollider2D cd;

    [Header("Actions")]
    public static Action onDeath;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        health = GetComponent<PlayerHealth>();
        damageHandler = GetComponent<PlayerDamageHandler>();
        cd = GetComponent<CapsuleCollider2D>();

        CharacterSelectionManager.onCharacterSelected += CharacterSelectCallback;
    }

    private void OnDisable()
    {
        CharacterSelectionManager.onCharacterSelected -= CharacterSelectCallback;
    }

    public void TakeDamage(int damage)
    {
        int realDamage = damageHandler.CalculateDamage(damage);

        if (realDamage != 0)
            health.ApplyDamage(realDamage);
    }

    public void Die()
    {
        onDeath?.Invoke();
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    public Vector2 GetCenterPoint()
    {
        return (Vector2)transform.position + new Vector2(0, cd.offset.y + cd.size.y / 2);
    }

    private void CharacterSelectCallback(CharacterDataSO characterData)
    {
        playerRenderer.sprite = characterData.Sprite;
    }
}
