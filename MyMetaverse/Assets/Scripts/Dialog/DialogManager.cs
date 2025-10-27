using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogState
{
    None,
    Typing,
    TypingDone,
    DialogDone
}

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogUI dialogUI;

    private Queue<string> dialogQueue = new Queue<string>();

    private Dialog currentDialog;
    private DialogState currentState;
    private bool isFinishDioalog;

    private void Start()
    {
        currentState = DialogState.None;
        dialogUI.gameObject.SetActive(false);
        isFinishDioalog = false;
    }

    public void SetDialog(Dialog dialog)
    {
        currentDialog = dialog;

        dialogUI.gameObject.SetActive(true);

        dialogQueue.Clear();

        // Dialog NPC 이미지와 이름 설정
        dialogUI.SetDialogTitle(currentDialog.sprite, currentDialog.name, currentDialog.optionList, currentDialog.optionIdxList);

        // Dialog Message List Queue에 넣기
        SetDialogQueue(currentDialog.dialogList);
    }

    private void SetDialogQueue(List<string> messageList)
    {
        foreach (string message in messageList)
            dialogQueue.Enqueue(message);
    }

    public bool StartDialogTyping()
    {
        if (dialogQueue.Count <= 0) return true;

        currentState = DialogState.Typing;

        string message = dialogQueue.Dequeue();
        dialogUI.StartDialog(message);

        return false;
    }

    public void FinishDialog()
    {
        // Dialog Typing이 끝나면 TypingDone로 변환
        currentState = DialogState.TypingDone;
    }

    /// <summary>
    /// 대화 중일때 사용자가 Space나 마우스 버튼 누를 시 호출
    /// </summary>
    public void DialogInteract()
    {
        // 현재 대화 타이핑이 끝난 상태라면 상호작용
        if(currentState == DialogState.TypingDone)
        {
            // 이어서 대화 타이핑
            bool isDone = StartDialogTyping();

            // 모든 대화가 끝나면 동작
            if (isDone)
            {
                // 후열 대화면 그대로 종료
                if (isFinishDioalog)
                {
                    isFinishDioalog = false;
                    dialogUI.gameObject.SetActive(false);
                    currentState = DialogState.None;
                    GameManager.Instance.ChangeGameState(GameState.Playing);
                }
                else
                {
                    // 아니라면 Option 창 보여주기
                    dialogUI.ShowAllOptionBtn();
                    currentState = DialogState.DialogDone;
                }
  
            }
        }
    }

    public void OnOptionSelected(int index)
    {
        GameManager.Instance.ChangeGameState(GameState.Playing);
        dialogUI.gameObject.SetActive(false);
        currentState = DialogState.None;
        switch (index)
        {
            case 0:
                {
                    dialogUI.gameObject.SetActive(true);
                    isFinishDioalog = true;
                    dialogUI.CloseAllOptionBtn();
                    dialogUI.gameObject.SetActive(true);

                    // 후열 대화 시작
                    SetDialogQueue(currentDialog.backRowDialogList);
                    StartDialogTyping();

                    GameManager.Instance.ChangeGameState(GameState.Dialog);
                }
                break;
            case 1:
                // CustomizingUI 보여주기
                GameManager.Instance.ChangeGameState(GameState.Event);
                CustomizeManager.Instance.OpenCustomizingUI();
                CustomizeManager.Instance.InitCustomizing();
                break;
            case 2:
                GameManager.Instance.StartMiniGame();
                break;
            case 3:
                GameManager.Instance.StartMiniGame2();
                break;
            default:
                Debug.LogWarning("There is No Dialog Option Click Event");
                break;
        }
    }
}
