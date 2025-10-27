using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameBaseUI : MonoBehaviour
{
    [Header("StartUI")]
    [SerializeField] protected GameObject infoUI;
    [SerializeField] protected GameObject readyUI;
    [SerializeField] protected TextMeshProUGUI readyCountTxt;

    [Header("CanvasUI")]
    [SerializeField] protected GameObject coinUI;
    [SerializeField] protected GameObject scoreUI;
    [SerializeField] protected GameObject gameOverUI;
    [SerializeField] protected TextMeshProUGUI bestRecord;
    [SerializeField] protected TextMeshProUGUI currentRecord;
    [SerializeField] protected TextMeshProUGUI gainCoin;

    protected virtual void Start()
    {
        // canvas 초기화
        infoUI.SetActive(true);
        gameOverUI.SetActive(false);
        readyUI.SetActive(false);
        scoreUI.SetActive(false);
        coinUI.SetActive(false);
    }

    public virtual void StartReady(int readyCount)
    {
        infoUI.SetActive(false);

        // readyUI 보여주고 readyCount 만큼 카운트 시작
        StartCoroutine(ReadyCoroutine(readyCount));
    }

    public virtual IEnumerator ReadyCoroutine(int readyCount)
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
