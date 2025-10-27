using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FallSurviveUI : MonoBehaviour
{
    [Header("StartUI")]
    [SerializeField] private GameObject infoUI;
    [SerializeField] private GameObject readyUI;
    [SerializeField] private TextMeshProUGUI readyCountTxt;

    [Header("CanvasUI")]
    [SerializeField] private GameObject coinUI;
    [SerializeField] private GameObject scoreUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI bestRecord;
    [SerializeField] private TextMeshProUGUI currentRecord;
    [SerializeField] private TextMeshProUGUI gainCoin;

    // GameManager
    private FallSurviveManager gameManager;


    private void Start()
    {
        // canvas 초기화
        infoUI.SetActive(true);
        gameOverUI.SetActive(false);
        readyUI.SetActive(false);
        scoreUI.SetActive(false);
        coinUI.SetActive(false);

        this.gameManager = GameManager.Instance.CurrentMiniGame<FallSurviveManager>();
    }

    public void StartReady(int readyCount)
    {
        infoUI.SetActive(false);

        // readyUI 보여주고 readyCount 만큼 카운트 시작
        StartCoroutine(ReadyCoroutine(readyCount));
    }

    IEnumerator ReadyCoroutine(int readyCount)
    {
        readyUI.SetActive(true);

        while (readyCount > 0)
        {
            readyCountTxt.text = readyCount.ToString();
            readyCount--;

            yield return new WaitForSeconds(1f);
        }

        // 끝나면 ReadyUI 지우고 게임 시작
        readyUI.SetActive(false);
        scoreUI.SetActive(true);
        coinUI.SetActive(true);
        gameManager.GameStart();
    }

    public void UpdateScore(int score)
    {
        if (scoreUI == null) return;

        TextMeshProUGUI scoreTxt = scoreUI.GetComponentInChildren<TextMeshProUGUI>();
        scoreTxt.text = $"{score}점";
    }

    public void UpdateCoin(int coin)
    {
        TextMeshProUGUI coinTxt = coinUI.GetComponentInChildren<TextMeshProUGUI>();
        coinTxt.text = coin.ToString();
    }

    public void GameOver(int bestScore, int score, int coin)
    {
        gameOverUI.SetActive(true);
        bestRecord.text = $"{bestScore}점";
        currentRecord.text = $"{score}점";
        gainCoin.text = coin.ToString();
    }
}
