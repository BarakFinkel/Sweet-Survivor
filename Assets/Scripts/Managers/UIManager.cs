using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject weaponSelectPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private GameObject chestOpenPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

    private List<GameObject> panels = new List<GameObject>();

    private void Awake()
    {
        panels.AddRange(new GameObject[]
        {
            menuPanel,
            weaponSelectPanel,
            gamePanel,
            levelUpPanel,
            chestOpenPanel,
            shopPanel,
            stageCompletePanel,
            gameOverPanel
        });
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;

            case GameState.WEAPONSELECT:
                ShowPanel(weaponSelectPanel);
                break;

            case GameState.GAME:
                ShowPanel(gamePanel);
                break;

            case GameState.LEVELUP:
                ShowPanel(levelUpPanel);
                break;

            case GameState.CHESTOPEN:
                ShowPanel(chestOpenPanel);
                break;

            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;

            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;

            case GameState.GAMEOVER:
                ShowPanel(gameOverPanel);
                break;            
        }
    }

    private void ShowPanel(GameObject newPanel, bool hidePreviousPanels = true)
    {
        if (hidePreviousPanels)
        {
            foreach (GameObject panel in panels)
                panel.SetActive(panel == newPanel);
        }
        else
        {
            newPanel.SetActive(true);
        }
    }
}
