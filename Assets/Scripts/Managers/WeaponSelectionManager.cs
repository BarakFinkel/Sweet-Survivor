using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionManager : MonoBehaviour
{
    public static WeaponSelectionManager instance;
    
    [Header("Elements")]
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private WeaponSelectionButton buttonPrefab;
    [SerializeField] private Button startButton;
    
    [Header("Data")]
    [SerializeField] private WeaponDataSO[] starterWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;
    private HashSet<WeaponDataSO> generatedWeapons = new HashSet<WeaponDataSO>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        GameManager.onGameStateChanged += GameStateChangedCallback;

        startButton.interactable = false;
    }

    void OnDisable()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;   
    }

    // Callback for when the game state changes to Weapon Selection.
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WEAPONSELECT:
                Configure();
                break;

            case GameState.GAME:
                
                if (selectedWeapon == null)
                    return;
                
                PlayerWeaponsManager.instance.TryAddWeapon(selectedWeapon, initialWeaponLevel);

                selectedWeapon     = null;
                initialWeaponLevel = 0;

                break;
        }
    }

    private void Configure()
    {
        // Clear all current weapon selection buttons and previously selected weapons.
        buttonsContainer.Clear();
        generatedWeapons.Clear();

        // Generate 3 weapon buttons.
        for (int i = 0; i < 3; i++)
            GenerateWeaponChoice();
    }

    private void GenerateWeaponChoice()
    {
        WeaponSelectionButton buttonInstance = Instantiate(buttonPrefab, buttonsContainer);
        WeaponDataSO weaponData;
        
        while (true)
        {
            weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];
            
            if (!generatedWeapons.Contains(weaponData))
            {
                generatedWeapons.Add(weaponData);
                break;
            }
        }

        int level = Random.Range(1, 3);

        buttonInstance.Configure(weaponData, level);

        // Remove all previous listeners from the button and add the invokation of the method above 
        // + a callback to set the button's onClick callback.
        buttonInstance.Button.onClick.RemoveAllListeners();
        buttonInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(buttonInstance, weaponData, level));
        buttonInstance.Button.onClick.AddListener(AudioManager.instance.PlayButtonSound);
    }

    private void WeaponSelectedCallback(WeaponSelectionButton weaponButton, WeaponDataSO weaponData, int level)
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = level;

        foreach (WeaponSelectionButton button in buttonsContainer.GetComponentsInChildren<WeaponSelectionButton>())
        {
            if (button == weaponButton)
            {
                button.Select();
            }
            else
            {
                button.Deselect();
            }
        }

        if (!startButton.interactable)
            startButton.interactable = true;
    }
}
