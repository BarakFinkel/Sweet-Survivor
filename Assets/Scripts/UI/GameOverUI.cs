using UnityEngine;
using TMPro;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI gameOverDetails;

    public void Awake()
    {
        GameManager.onGameStateChanged += SetGameOverDetails;
    }

    public void OnDestroy()
    {
        GameManager.onGameStateChanged -= SetGameOverDetails;
    }

    public void SetGameOverDetails(GameState gameState)
    {
        if (gameState == GameState.GAMEOVER)
        {
            gameOverDetails.text =
                "You Survived: "
                + WaveManager.instance.GetWavesSurvived()
                + "/"
                + WaveManager.instance.GetWavesTotal()
                + " Waves";
        }
    }
}
