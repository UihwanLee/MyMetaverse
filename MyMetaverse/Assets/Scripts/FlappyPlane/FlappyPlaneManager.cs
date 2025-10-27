using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MiniGameState
{
    Ready,
    GameStart
}

public class FlappyPlaneManager : MiniGameSingleton<FlappyPlaneManager>
{
    private GameObject player;

    public override string MiniGameName => "FlappyPlaneGameManager";

    private readonly string bestScoreKey = "BestScore";

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.SetCurrentMiniGame(this);
    }

    public override void Start()
    {
        base.Start();

        player = FindObjectOfType<PlaneController>().transform.gameObject;
    }

    public override void CheckRecord()
    {
        bestScore = PlayerPrefs.GetInt(bestScoreKey);
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt(bestScoreKey, currentScore);
        }
        bestScore = PlayerPrefs.GetInt(bestScoreKey);
    }

    public override GameObject GetPlayerObject()
    {
        return player;
    }
}
