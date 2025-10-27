using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSurviveManager : MiniGameSingleton<FallSurviveManager>
{
    private int bestScore;
    private int currentScore;
    private int currentCoin;

    private FallSurviveUI gameUI;
    public FallSurviveUI GameUI { get { return gameUI; } }

    private ExplorerController player;

    [SerializeField] private int readyCount = 3;

    public override string MiniGameName => "FallSurvivGameManager";

    private readonly string bestScoreKey = "BestFallSurviveScore";

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.SetCurrentMiniGame(this);
    }

    private void Start()
    {
        gameUI = FindObjectOfType<FallSurviveUI>();
        player = FindObjectOfType<ExplorerController>();

        currentScore = 0;
        bestScore = 0;
    }

    public void GameReady()
    {
        gameUI.StartReady(readyCount);
    }

    public void GameStart()
    {
        currentGameState = MiniGameState.GameStart;
    }

    public void AddScore(int score)
    {
        currentScore += score;
        gameUI.UpdateScore(currentScore);
    }

    public void AddCoin(int coin)
    {
        currentCoin += coin;
        gameUI.UpdateCoin(currentCoin);
    }

    public void GameOver()
    {
        // ���� ����� ��� ���� �ߴ��� üũ
        CheckRecord();

        // ���� ���� �� ���� ������ �ְ� ������ �Ѱ��ش�.
        gameUI.GameOver(bestScore, currentScore, currentCoin);
    }

    private void CheckRecord()
    {
        bestScore = PlayerPrefs.GetInt(bestScoreKey);
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt(bestScoreKey, currentScore);
        }
        bestScore = PlayerPrefs.GetInt(bestScoreKey);
    }

    public void RestartGame()
    {
        SceneController.Instance.LoadScene(2);
    }

    public void Return()
    {
        // GameManager���� Coin ���� ����
        GameManager.Instance.UpdateCoin(currentCoin);

        // GameManager State ����
        GameManager.Instance.ChangeGameState(GameState.Playing);

        SceneController.Instance.LoadScene(0);
    }
}
