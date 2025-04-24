using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    
    [Header("Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private AudioClip damageVolumeSound;
    [Range(0.0f, 1.0f)] [SerializeField] private float damageSoundVolume;
    private AudioSource audioSource;
    
    [Header("Settings")]
    [SerializeField] private float healthRegenTickCooldown = 1.0f;

    // Overall HP settings
    private int maxHealth;
    private int currentHealth;

    // HP Regen Settings
    private float healthRegen;
    private float regenTimer;

    private Queue<(int amount, bool isDamage)> healthEventQueue = new Queue<(int amount, bool isDamage)>();

    private bool dead;

    [Header("Actions")]
    public static Action onDamage;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = damageVolumeSound;
        audioSource.volume = damageSoundVolume;

        PlayerStatsManager.onStatsChanged += UpdateStats;        
    }

    void Update()
    {
        HandleHealthRegen();
        HandleHealthEvents();
    }

    private void OnDisable()
    {
        PlayerStatsManager.onStatsChanged -= UpdateStats; 
    }

    /// <summary>
    /// Handles current events and applying necessary changes to 
    /// </summary>
    public void HandleHealthEvents()
    {
        int totalHealthChange = 0;

        // If there're any events, we handle them and add/subtract their values from the var above.
        while (healthEventQueue.Count > 0)
        {
            var healthEvent = healthEventQueue.Dequeue();

            if (healthEvent.Item2) // Damage
            {
                totalHealthChange -= healthEvent.Item1;
                PlayDamageSound();
                onDamage?.Invoke();
            }
            else // Healing
            {
                totalHealthChange += healthEvent.Item1;
            }
        }

        // If any change is required
        if (totalHealthChange != 0)
        {
            currentHealth = Mathf.Clamp(currentHealth + totalHealthChange, 0, maxHealth);
            UpdateHealthUI();
        }

        if (currentHealth == 0 && !dead)
        {
            Player.instance.Die();
            dead = true;
        }   
    }

    /// <summary>
    /// Queues a damage instance within the player's health event queue.
    /// </summary>
    /// <param name="damage">The ammount of damage the instance will apply</param>
    public void ApplyDamage(int damageAmount)
    {
        if (damageAmount <= 0)
        {
            Debug.LogWarning($"Invalid damage value: {damageAmount} detected in ApplyDamage() method.");
            return;
        }

        healthEventQueue.Enqueue((damageAmount, true));
    }

    /// <summary>
    /// Queues a heal instance within the player's health event queue.
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyHeal(int healAmount)
    {
        if (healAmount <= 0)
        {
            Debug.LogWarning($"Invalid heal value: {healAmount} detected in ApplyHeal() method.");
            return;
        }

        healthEventQueue.Enqueue((healAmount, false));
    }

    public void HandleHealthRegen()
    {
        if(regenTimer == 0)
        {
            int regenAmount = Mathf.RoundToInt(healthRegen * maxHealth);
            
            if(regenAmount > 0)
                ApplyHeal(regenAmount);

            regenTimer = healthRegenTickCooldown;
        }
        else
        {
            regenTimer = Mathf.Max(regenTimer - Time.deltaTime, 0);
        }
    }

    public void UpdateHealthUI()
    {
        healthSlider.value = (float)currentHealth / maxHealth;
        healthText.text = currentHealth + " / " + maxHealth; 
    }

    public void UpdateStats()
    {
        // Health Update:
        // Also adds the delta added to the maxHP if any changes were made to the 

        int newMaxHealth = (int)PlayerStatsManager.instance.GetStatValue(Stat.MaxHealth);
        int healthDelta = newMaxHealth - maxHealth;

        maxHealth = newMaxHealth;
        currentHealth += healthDelta;

        UpdateHealthUI();

        healthRegen = PlayerStatsManager.instance.GetStatValue(Stat.HealthRegen) / 100;
    }

    public void PlayDamageSound()
    {
        if (!AudioManager.instance.IsSFXOn || audioSource.clip == null)
            return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }
}
