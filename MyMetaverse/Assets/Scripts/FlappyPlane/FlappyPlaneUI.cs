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
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI bestRecord;
    [SerializeField] private TextMeshProUGUI currentRecord;


    private void Start()
    {
        // canvas 초기화
        infoUI.SetActive(true);
        gameOverUI.SetActive(false);
        readyUI.SetActive(false);
        scoreTxt.transform.gameObject.SetActive(false);
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

        while(readyCount > 0)
        {
            readyCountTxt.text = readyCount.ToString();
            readyCount--;

            yield return new WaitForSeconds(1f);
        }

        // 끝나면 ReadyUI 지우고 게임 시작
        readyUI.SetActive(false);
        scoreTxt.transform.gameObject.SetActive(true);
        FlappyPlaneManager.Instance.GameStart();
    }

    public void UpdateScore(int score)
    {
        scoreTxt.text = $"{score}점";
    }

    public void GameOver(int bestScore, int score)
    {
        gameOverUI.SetActive(true);
        bestRecord.text = $"{bestScore}점";
        currentRecord.text = $"{score}점";
    }
}
