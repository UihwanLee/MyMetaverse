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

        // Dialog NPC �̹����� �̸� ����
        dialogUI.SetDialogTitle(currentDialog.sprite, currentDialog.name, currentDialog.optionList, currentDialog.optionIdxList);

        // Dialog Message List Queue�� �ֱ�
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
        // Dialog Typing�� ������ TypingDone�� ��ȯ
        currentState = DialogState.TypingDone;
    }

    /// <summary>
    /// ��ȭ ���϶� ����ڰ� Space�� ���콺 ��ư ���� �� ȣ��
    /// </summary>
    public void DialogInteract()
    {
        // ���� ��ȭ Ÿ������ ���� ���¶�� ��ȣ�ۿ�
        if(currentState == DialogState.TypingDone)
        {
            // �̾ ��ȭ Ÿ����
            bool isDone = StartDialogTyping();

            // ��� ��ȭ�� ������ ����
            if (isDone)
            {
                // �Ŀ� ��ȭ�� �״�� ����
                if (isFinishDioalog)
                {
                    isFinishDioalog = false;
                    dialogUI.gameObject.SetActive(false);
                    currentState = DialogState.None;
                    GameManager.Instance.ChangeGameState(GameState.Playing);
                }
                else
                {
                    // �ƴ϶�� Option â �����ֱ�
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

                    // �Ŀ� ��ȭ ����
                    SetDialogQueue(currentDialog.backRowDialogList);
                    StartDialogTyping();

                    GameManager.Instance.ChangeGameState(GameState.Dialog);
                }
                break;
            case 1:
                // CustomizingUI �����ֱ�
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
