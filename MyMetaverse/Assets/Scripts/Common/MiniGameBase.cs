using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    protected GameManager mainGameManager;

    public abstract string MiniGameName { get; }

    protected MiniGameState currentGameState;
    public MiniGameState CurrentGameState { get { return currentGameState; } set { currentGameState = value; } }

    private int bestScore;
    private int currentScore;
    private int currentCoin;

    [SerializeField] private int readyCount = 3;

    public virtual void Init(GameManager gm)
    {
        mainGameManager = gm;
    }

    public virtual void GameReady()
    {
        //gameUI.StartReady(readyCount);
    }

    public virtual void GameStart()
    {
        currentGameState = MiniGameState.GameStart;
    }

    public virtual void AddScore(int score)
    {
        currentScore += score;
        //gameUI.UpdateScore(currentScore);
    }

    public virtual void AddCoin(int coin)
    {
        currentCoin += coin;
        //gameUI.UpdateCoin(currentCoin);
    }

    public virtual void GameOver()
    {
        // ���� ����� ��� ���� �ߴ��� üũ
        CheckRecord();

        // ���� ���� �� ���� ������ �ְ� ������ �Ѱ��ش�.
        //gameUI.GameOver(bestScore, currentScore, currentCoin);
    }

    private void CheckRecord()
    {
        //bestScore = PlayerPrefs.GetInt(bestScoreKey);
        //if (currentScore > bestScore)
        //{
        //    PlayerPrefs.SetInt(bestScoreKey, currentScore);
        //}
        //bestScore = PlayerPrefs.GetInt(bestScoreKey);
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

    public virtual void Exit()
    {
        Debug.Log($"{MiniGameName} �����");
    }
}
