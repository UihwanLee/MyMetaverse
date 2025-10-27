using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyPlaneUI : MiniGameBaseUI
{
    // GameManager
    private FlappyPlaneManager gameManager;

    protected override void Start()
    {
        base.Start();
        this.gameManager = GameManager.Instance.CurrentMiniGame<FlappyPlaneManager>();
    }

    public override void StartReady(int readyCount)
    {
        base.StartReady(readyCount);
    }

    public override IEnumerator ReadyCoroutine(int readyCount)
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
}
