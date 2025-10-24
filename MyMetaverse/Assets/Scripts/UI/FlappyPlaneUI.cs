using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyPlaneUI : MonoBehaviour
{
    [Header("CanvasUI")]
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI bestRecord;
    [SerializeField] private TextMeshProUGUI currentRecord;

    private void Start()
    {
        // canvas 초기화
        gameOverUI.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreTxt.text = $"{score}점";
    }

    public void GameOver(int bestScore, int score)
    {
        gameOverUI.SetActive(true);
        bestRecord.text = $"{bestScore}점";
        currentRecord.text = $"{score}점";
    }
}
