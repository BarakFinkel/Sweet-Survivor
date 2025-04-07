using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class LevelingManager : MonoBehaviour, IGameStateListener
{
    public static LevelingManager instance;
    
    [Header("Elements")]
    [SerializeField] private UpgradeButton[] upgradeButtons;

    [Header("Stat Upgrade Ranges")]
    [SerializeField] private Vector2 attackRange = new Vector2(1, 10);
    [SerializeField] private Vector2 attackSpeedRange = new Vector2(1, 10);
    [SerializeField] private Vector2 critChanceRange = new Vector2(1, 10);
    [SerializeField] private Vector2 critDamageRange = new Vector2(1f, 2f);
    [SerializeField] private Vector2 moveSpeedRange = new Vector2(1, 10);
    [SerializeField] private Vector2 maxHealthRange = new Vector2(1, 5);
    [SerializeField] private Vector2 rangeRange = new Vector2(1f, 5f);
    [SerializeField] private Vector2 healthRegenRange = new Vector2(1, 10);
    [SerializeField] private Vector2 armorRange = new Vector2(1, 10);
    [SerializeField] private Vector2 luckRange = new Vector2(1, 10);
    [SerializeField] private Vector2 dodgeChanceRange = new Vector2(1, 10);
    [SerializeField] private Vector2 lifestealRange = new Vector2(1, 10);

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);   
    }

    /// <summary>
    /// A callback that's called whenever the GameManager changes the game state.
    /// Within this component, handles configuring the buttons only if the game state is LEVELUP.
    /// </summary>
    /// <param name="gameState"></param>
    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.LEVELUP)
            ConfigureUpgradeButtons();
    }

    /// <summary>
    /// A method to be called when the player levels up.
    /// Configures the buttons functionality to display available stats for upgrade,
    /// as well as updating their UI accordingly.
    /// </summary>
    private void ConfigureUpgradeButtons()
    {
        HashSet<Stat> chosenStats = new HashSet<Stat>();
        
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            // Keep trying to randomly get a stat type for upgrade untill we get one that wasn't already chosen.
            Stat randomStat;
            do
            {
                int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
                randomStat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            } 
            while (chosenStats.Contains(randomStat));

            // Add it and get it's icon and formatted name for display.
            chosenStats.Add(randomStat);
            Sprite statSprite = ResourceManager.GetStatIcon(randomStat);
            string randomStatName = PlayerStatsManager.FormatStatName(randomStat);

            // Generate the correct stat increasing method for execution upon the player's choice of upgrade.
            string buttonString;
            Action action = GetAction(randomStat, out buttonString);

            // Configure the button's visuals accordingly.
            upgradeButtons[i].Configure(statSprite, randomStatName, buttonString);
            
            // Remove all previous listeners from the button and add the invokation of the method above 
            // + a callback to set the game back to the Game state.
            upgradeButtons[i].Button.onClick.RemoveAllListeners();
            upgradeButtons[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradeButtons[i].Button.onClick.AddListener(() => GameManager.instance.SetGameState(GameState.GAME));
        }
    }

    /// <summary>
    /// A method to generate an action to be invoked upon choosing the corresponding stat upgrade.
    /// </summary>
    /// <param name="stat">The generated stat for possible improvment.</param>
    /// <param name="buttonString">The value text to be displayed within the level-up UI.</param>
    /// <returns></returns>
    private Action GetAction(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value;

        Vector2 range = stat switch
        {
            Stat.Attack => attackRange,
            Stat.AttackSpeed => attackSpeedRange,
            Stat.CriticalChance => critChanceRange,
            Stat.CriticalDamage => critDamageRange,
            Stat.MoveSpeed => moveSpeedRange,
            Stat.MaxHealth => maxHealthRange,
            Stat.Range => rangeRange,
            Stat.HealthRegen => healthRegenRange,
            Stat.Armor => armorRange,
            Stat.Luck => luckRange,
            Stat.DodgeChance => dodgeChanceRange,
            Stat.Lifesteal => lifestealRange,
            _ => new Vector2(0, 0)
        };

        value = Random.Range(range.x, range.y);

        if (stat == Stat.CriticalDamage || stat == Stat.Range)
        {
            buttonString = "+ " + value.ToString("F2") + (stat == Stat.CriticalDamage ? "x" : "");
        }
        else
        {
            int naturalValue = Mathf.RoundToInt(value);
            value = naturalValue;

            buttonString = "+ " + naturalValue + ((stat == Stat.MaxHealth || stat == Stat.Attack) ? "" : "%");
        }

        return () => PlayerStatsManager.instance.AddToStat(stat, value);
    }
}
