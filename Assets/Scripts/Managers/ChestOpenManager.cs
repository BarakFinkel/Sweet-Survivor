using UnityEngine;

using Random = UnityEngine.Random;

public class ChestOpenManager : MonoBehaviour
{
    public static ChestOpenManager instance;

    [Header("Player")]
    [SerializeField] private PlayerObjectsManager objectsManager;

    [Header("Elements")]
    [SerializeField] private ChestObjectContainer containerPrefab;
    [SerializeField] private Transform containerParent;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        GameManager.onGameStateChanged += GameStateChangedCallback;
        ChocolateChest.onCollect       += ChestCollectedCallback;
    }

    public void OnDisable()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        ChocolateChest.onCollect       -= ChestCollectedCallback;
    }

    private void OpenChest()
    {
        containerParent.Clear();

        ObjectDataSO[] objectDatas    = ResourceManager.Objects;
        ObjectDataSO randomObjectData = objectDatas[Random.Range(0, objectDatas.Length)];

        ChestObjectContainer instance = Instantiate(containerPrefab, containerParent);
        instance.Configure(randomObjectData);

        instance.EquipButton.onClick.RemoveAllListeners();

        instance.EquipButton.onClick.AddListener(() => EquipButtonCallback(randomObjectData));
        instance.EquipButton.onClick.AddListener(() => GameManager.instance.SetGameState(GameState.GAME));

        instance.MeltButton.onClick.AddListener(() => MeltButtonCallback(randomObjectData));
        instance.MeltButton.onClick.AddListener(() => GameManager.instance.SetGameState(GameState.GAME));
    }

    private void EquipButtonCallback(ObjectDataSO objectForTaking) => objectsManager.AddObject(objectForTaking);

    private void MeltButtonCallback(ObjectDataSO objectForMelting)
    {
        CurrencyManager.instance.AddCurrency(CurrencyType.Normal, objectForMelting.RecyclePrice);
    }

    /// <summary>
    /// A callback that's called whenever the GameManager changes the game state.
    /// Within this component, handles configuring the buttons only if the game state is LEVELUP.
    /// </summary>
    /// <param name="gameState"></param>
    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.CHESTOPEN)
            OpenChest();
    }

    private void ChestCollectedCallback(ChocolateChest chest) => GameManager.instance.SetGameState(GameState.CHESTOPEN);
}
