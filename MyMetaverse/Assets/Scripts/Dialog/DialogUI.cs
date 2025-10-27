using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [Header("Dialog UI")]
    [SerializeField] private Image npcImg;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI dialogMessage;
    [SerializeField] private List<Button> optionBtnList;

    [Header("Dialog Manager")]
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private TypingEffect typer;

    [SerializeField] private int optionCount;

    private void Start()
    {
        typer = GetComponent<TypingEffect>();
    }

    public void SetDialogTitle(Sprite sprite, string name, List<string> optionList, List<int> optionIdxList)
    {
        // UI Setting
        npcImg.sprite = sprite;
        npcName.text = name;

        // Option Btn Setting
        SetDialogOptionBtn(optionList, optionIdxList);
    }

    private void SetDialogOptionBtn(List<string> optionList, List<int> optionIdxList)
    {
        this.optionCount = optionList.Count;
        for (int i = 0; i < optionBtnList.Count; i++)
        {
            if (i < optionCount)
            {
                int buttonIndex = i;
                optionBtnList[i].gameObject.SetActive(true);
                optionBtnList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = optionList[i];
                optionBtnList[i].onClick.RemoveAllListeners();
                optionBtnList[i].onClick.AddListener(() =>
                {
                    // DialogManager�� OnOptionSelected ȣ���ϵ��� ����
                    dialogManager.OnOptionSelected(optionIdxList[buttonIndex]);
                });
                optionBtnList[i].gameObject.SetActive(false);
            }
            else
                optionBtnList[i].gameObject.SetActive(false);
        }
    }

    public void StartDialog(string message)
    {
        // Ÿ���� ����Ʈ�� �����ְ� �ൿ��ȯ
        typer.Typing(dialogMessage, message, onCompleted: () =>
        {
            dialogManager.FinishDialog();
        });
    }

    public void ShowAllOptionBtn()
    {
        for (int i=0; i<optionCount; i++)
        {
            optionBtnList[i].gameObject.SetActive(true);
        }
    }

    public void CloseAllOptionBtn()
    {
        for (int i = 0; i < optionCount; i++)
        {
            optionBtnList[i].gameObject.SetActive(false);
        }
    }
}
