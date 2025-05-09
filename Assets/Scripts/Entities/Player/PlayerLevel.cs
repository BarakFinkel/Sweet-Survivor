using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI lvlText;
    
    [Header("Settings")]
    [SerializeField] private int initRequiredXP = 10;
    [SerializeField] private float xpRequirementExponent = 1.1f;
    private int requiredXP;
    private int currentXP = 0;
    private int level = 1;

    void Start()
    {
        requiredXP = initRequiredXP;
        UpdateUI();
    }

    void OnEnable()
    {
        SodaXP.onCollect += SodaCollectedCallback;
    }

    void OnDisable()
    {
        SodaXP.onCollect -= SodaCollectedCallback;
    }

    private void UpdateUI()
    {
        xpBar.value = (float)currentXP / requiredXP;
        lvlText.text = "Level " + level;
    }

    private void UpdateRequiredXP()
    {
        requiredXP = Mathf.RoundToInt(Mathf.Pow(requiredXP, xpRequirementExponent));
    }

    private void SodaCollectedCallback(SodaXP soda)
    {
        currentXP++;

        if (currentXP >= requiredXP)
            LevelUp();

        UpdateUI();
    }

    private void LevelUp()
    {
        level++;
        currentXP = 0;
        UpdateRequiredXP();
        GameManager.instance.SetGameState(GameState.LEVELUP);
    }
}
