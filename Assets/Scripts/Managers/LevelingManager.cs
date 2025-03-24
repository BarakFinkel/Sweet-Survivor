using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LevelingManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Button[] upgradeButtons;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.LEVELUP:
                ConfigureUpgradeButtons();
                break;
        }
    }

    private void ConfigureUpgradeButtons()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int randomIndex       = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat randomStat       = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            string randomStatName = PlayerStats.FormatStatName(randomStat);

            upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = randomStatName;
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => Debug.Log(randomStatName));
        }
    }
}
