using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyPlaneUI : MonoBehaviour
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


    private void Start()
    {
        // canvas �ʱ�ȭ
        infoUI.SetActive(true);
        gameOverUI.SetActive(false);
        readyUI.SetActive(false);
        scoreUI.SetActive(false);
        coinUI.SetActive(false);
    }

    public void StartReady(int readyCount)
    {
        infoUI.SetActive(false);

        // readyUI �����ְ� readyCount ��ŭ ī��Ʈ ����
        StartCoroutine(ReadyCoroutine(readyCount));
    }

    IEnumerator ReadyCoroutine(int readyCount)
    {
        readyUI.SetActive(true);

        while(readyCount > 0)
        {
            readyCountTxt.text = readyCount.ToString();
            readyCount--;

            yield return new WaitForSeconds(1f);
        }

        // ������ ReadyUI ����� ���� ����
        readyUI.SetActive(false);
        scoreUI.SetActive(true);
        coinUI.SetActive(true);
        FlappyPlaneManager.Instance.GameStart();
    }

    public void UpdateScore(int score)
    {
        TextMeshProUGUI scoreTxt = scoreUI.GetComponentInChildren<TextMeshProUGUI>();
        scoreTxt.text = $"{score}��";
    }

    public void UpdateCoin(int coin)
    {
        TextMeshProUGUI coinTxt = coinUI.GetComponentInChildren<TextMeshProUGUI>();
        coinTxt.text = coin.ToString();
    }

    public void GameOver(int bestScore, int score, int coin)
    {
        gameOverUI.SetActive(true);
        bestRecord.text = $"{bestScore}��";
        currentRecord.text = $"{score}��";
        gainCoin.text = coin.ToString();
    }
}
