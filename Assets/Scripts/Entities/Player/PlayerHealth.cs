using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int maxHealth;
    private int health;
    
    private Queue<int> damageQueue = new Queue<int>();
    
    void Start()
    {
        health = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        HandleDamage();
    }

    public void HandleDamage()
    {
        while (damageQueue.Count > 0)
        {
            int damage = damageQueue.Dequeue();
            health = Mathf.Max(health - damage, 0);
        }   

        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        damageQueue.Enqueue(damage);
    }

    private void Die()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    public void UpdateHealthUI()
    {
        healthSlider.value = (float)health / maxHealth;
        healthText.text = health + " / " + maxHealth; 
    }
}
