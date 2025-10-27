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

        // Dialog NPC �̹����� �̸� ����
        dialogUI.SetDialogTitle(dialog.sprite, dialog.name, dialog.optionList, dialog.optionIdxList);

        // Dialog Message List Queue�� �ֱ�
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

            // ��� ��ȭ�� ������ Option â �����ֱ�
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

        dialogUI.gameObject.SetActive(false);
        currentState = DialogState.None;
    }
}
