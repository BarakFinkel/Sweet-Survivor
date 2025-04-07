using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private WeaponSelectionButton buttonPrefab;
    private PlayerWeaponsManager playerWeapons;
    
    [Header("Data")]
    [SerializeField] private WeaponDataSO[] starterWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerWeapons = Player.instance.GetComponent<PlayerWeaponsManager>();
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
                
                playerWeapons.TryAddWeapon(selectedWeapon, initialWeaponLevel);
                
                selectedWeapon     = null;
                initialWeaponLevel = 0;

                break;
        }
    }

    private void Configure()
    {
        // Clear all current buttons in the container.
        buttonsContainer.Clear();

        // Generate 3 weapon buttons.
        for (int i = 0; i < 3; i++)
            GenerateWeaponChoice();
    }

    private void GenerateWeaponChoice()
    {
        WeaponSelectionButton buttonInstance = Instantiate(buttonPrefab, buttonsContainer);
        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

        int level = Random.Range(1, 5);

        buttonInstance.Configure(weaponData, level);

        // Remove all previous listeners from the button and add the invokation of the method above 
        // + a callback to set the button's onClick callback.
        buttonInstance.Button.onClick.RemoveAllListeners();
        buttonInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(buttonInstance, weaponData, level));
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
    }
}
