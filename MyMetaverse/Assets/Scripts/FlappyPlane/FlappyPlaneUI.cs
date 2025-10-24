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
        // canvas �ʱ�ȭ
        gameOverUI.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreTxt.text = $"{score}��";
    }

    public void GameOver(int bestScore, int score)
    {
        gameOverUI.SetActive(true);
        bestRecord.text = $"{bestScore}��";
        currentRecord.text = $"{score}��";
    }
}
