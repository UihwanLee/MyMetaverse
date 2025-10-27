using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallSurviveManager : MiniGameSingleton<FallSurviveManager>
{
    private GameObject player;

    public override string MiniGameName => "FallSurvivGameManager";

    private readonly string bestScoreKey = "BestFallSurviveScore";

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.SetCurrentMiniGame(this);
    }

    public override void Start()
    {
        base.Start();

        player = FindObjectOfType<ExplorerController>().transform.gameObject;
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
