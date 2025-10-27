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

    private DialogState currentState;

    private void Start()
    {
        currentState = DialogState.None;
        dialogUI.gameObject.SetActive(false);
    }

    public void SetDialog(Dialog dialog)
    {
        dialogUI.gameObject.SetActive(true);

        dialogQueue.Clear();

        // Dialog NPC 이미지와 이름 설정
        dialogUI.SetDialogTitle(dialog.sprite, dialog.name, dialog.optionList, dialog.optionIdxList);

        // Dialog Message List Queue에 넣기
        List<string> messageList = dialog.dialogList;
        foreach(string message in messageList) 
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

            // 모든 대화가 끝나면 Option 창 보여주기
            if (isDone)
            {
                dialogUI.ShowAllOptionBtn();
                currentState = DialogState.DialogDone;
            }
        }
    }

    public void OnOptionSelected(int index)
    {
        GameManager.Instance.ChangeGameState(GameState.Playing);
        switch (index)
        {
            case 0:
                dialogUI.gameObject.SetActive(false);
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

        dialogUI.gameObject.SetActive(false);
        currentState = DialogState.None;
    }
}
