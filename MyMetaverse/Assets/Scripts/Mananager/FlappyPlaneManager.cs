using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyPlaneManager : MonoBehaviour
{
    private int bestScore;
    private int currentScore;

    private FlappyPlaneUI gameUI;
    public FlappyPlaneUI GameUI {  get { return gameUI; } }

    private PlaneController plane;

    [SerializeField] private float countdownTime = 3f;

    private static FlappyPlaneManager instance;
    public static FlappyPlaneManager Instance { get { return instance; } }

    private readonly string bestScoreKey = "BestScore";

    private void Awake()
    {
        instance = this;

        gameUI = FindObjectOfType<FlappyPlaneUI>();
        plane = FindObjectOfType<PlaneController>();

        if (gameUI == null)
            Debug.LogWarning("There is no FlappyPlaneUI");

        if (plane == null)
            Debug.LogWarning("There is no PlaneController");

        currentScore = 0;
        bestScore = 0;
    }

    public void AddScore(int score)
    {
        currentScore += score;
        gameUI.UpdateScore(currentScore);
    }

    public void GameOver()
    {
        // ���� ����� ��� ���� �ߴ��� üũ
        CheckRecord();

        // ���� ���� �� ���� ������ �ְ� ������ �Ѱ��ش�.
        gameUI.GameOver(bestScore, currentScore);
    }

    private void CheckRecord()
    {
        bestScore = PlayerPrefs.GetInt(bestScoreKey);
        if(currentScore > bestScore)
        {
            PlayerPrefs.SetInt(bestScoreKey, currentScore);
        }
        bestScore = PlayerPrefs.GetInt(bestScoreKey);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}
