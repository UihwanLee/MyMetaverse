using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MiniGameBase : MonoBehaviour
{
    protected GameManager mainGameManager;

    public abstract string MiniGameName { get; }

    protected MiniGameState currentGameState;
    public MiniGameState CurrentGameState { get { return currentGameState; } set { currentGameState = value; } }

    protected int bestScore;
    protected int currentScore;
    protected int currentCoin;

    [SerializeField] protected int readyCount = 3;

    [SerializeField] protected MiniGameBaseUI gameUI;

    public virtual void Init(GameManager gm)
    {
        mainGameManager = gm;
    }

    public virtual void Start()
    {
        currentScore = 0;
        bestScore = 0;
    }

    public virtual void GameReady()
    {
        gameUI.StartReady(readyCount);
    }

    public virtual void GameStart()
    {
        currentGameState = MiniGameState.GameStart;
    }

    public virtual void AddScore(int score)
    {
        currentScore += score;
        gameUI.UpdateScore(currentScore);
    }

    public virtual void AddCoin(int coin)
    {
        currentCoin += coin;
        gameUI.UpdateCoin(currentCoin);
    }

    public virtual void GameOver()
    {
        // 현재 기록이 기록 갱신 했는지 체크
        CheckRecord();

        // 게임 종료 시 현재 점수와 최고 점수를 넘겨준다.
        gameUI.GameOver(bestScore, currentScore, currentCoin);
    }

    public virtual void CheckRecord()
    {
    }

    public void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneController.Instance.LoadScene(currentSceneIndex);
    }

    public void Return()
    {
        // GameManager에게 Coin 정보 전달
        GameManager.Instance.UpdateCoin(currentCoin);

        // GameManager State 변경
        GameManager.Instance.ChangeGameState(GameState.Playing);

        SceneController.Instance.LoadScene(0);
    }

    public virtual void Exit()
    {
        Debug.Log($"{MiniGameName} 종료됨");
    }
}
