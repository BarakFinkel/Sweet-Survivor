using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour, ISaveAndLoad
{
    public static CharacterSelectionManager instance;
    private const string unlockedCharactersKey = "UnlockedCharacters";
    private const string lastSelectedCharacterKey = "LastSelectedCharacter";

    [Header("Elements")]
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private CharacterButton buttonPrefab;
    [SerializeField] private Image centerCharacterImage;
    [SerializeField] private CharacterInfoPanel characterInfo;

    [Header("Data")]
    private CharacterDataSO[] characterDatas;
    private List<bool> unlockedCharacters = new List<bool>();

    [Header("Settings")]
    private int selectedCharacterIndex;
    private int lastSelectedCharacterIndex;

    [Header("Actions")]
    public static Action<CharacterDataSO> onCharacterSelected;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        characterInfo.Button.onClick.RemoveAllListeners();
        characterInfo.Button.onClick.AddListener(AudioManager.instance.PlayButtonSound);
        characterInfo.Button.onClick.AddListener(PurchaseSelectedCharacter);

        CharacterClickedCallback(lastSelectedCharacterIndex, false);
    }

    private void Initialize()
    {
        buttonsParent.Clear();
        
        for (int i = 0; i < characterDatas.Length; i++)
            CreateCharacterButton(i);
    }

    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatas[index];

        CharacterButton buttonInstance = Instantiate(buttonPrefab, buttonsParent);
        buttonInstance.Configure(characterData.Sprite, unlockedCharacters[index]);

        buttonInstance.Button.onClick.RemoveAllListeners();
        buttonInstance.Button.onClick.AddListener(() => CharacterClickedCallback(index, true));
    }

    private void CharacterClickedCallback(int index, bool playSFX)
    {
        if (playSFX)
            AudioManager.instance.PlayButtonSound();

        lastSelectedCharacterIndex = index;
        selectedCharacterIndex     = index;

        CharacterDataSO characterData = characterDatas[index];

        if (unlockedCharacters[index])
        {
            Save();
            onCharacterSelected?.Invoke(characterData);
        }

        centerCharacterImage.sprite = characterData.Sprite;
        characterInfo.Configure(characterData, unlockedCharacters[index]);
    }

    private void PurchaseSelectedCharacter()
    {
        int characterPrice = characterDatas[selectedCharacterIndex].PurchasePrice;
        CurrencyManager.instance.UseCurrency(CurrencyType.Premium, characterPrice);

        // Save unlocked state of character
        unlockedCharacters[selectedCharacterIndex] = true;

        // Update button visuals
        buttonsParent.GetChild(selectedCharacterIndex).GetComponent<CharacterButton>().Unlock();

        // Update the character info panel
        CharacterClickedCallback(selectedCharacterIndex, true);

        Save();
    }

    public void Load()
    {
        characterDatas = ResourceManager.Characters;

        for (int i = 0; i < characterDatas.Length; i++)
            unlockedCharacters.Add(i == 0);

        if (SaveManager.TryLoad(this, unlockedCharactersKey, out object unlockedCharactersObject))
            unlockedCharacters = (List<bool>)unlockedCharactersObject;

        if (SaveManager.TryLoad(this, lastSelectedCharacterKey, out object lastSelectedCharacterObject))
            lastSelectedCharacterIndex = (int)lastSelectedCharacterObject;

        Initialize(); 
    }

    public void Save()
    {
        SaveManager.Save(this, unlockedCharactersKey, unlockedCharacters);
        SaveManager.Save(this, lastSelectedCharacterKey, lastSelectedCharacterIndex);
    }
}
