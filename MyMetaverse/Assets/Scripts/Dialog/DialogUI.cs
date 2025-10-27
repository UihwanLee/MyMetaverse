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

    private int optionCount;

    private void Start()
    {
        optionCount = 0;
        typer = GetComponent<TypingEffect>();
    }

    public void SetDialogTitle(Sprite sprite, string name, List<string> optionList, List<int> optionIdxList)
    {
        // UI Setting
        npcImg.sprite = sprite;
        npcName.text = name;

        Debug.Log("끝??");

        // Option Btn Setting
        SetDialogOptionBtn(optionList, optionIdxList);
    }

    private void SetDialogOptionBtn(List<string> optionList, List<int> optionIdxList)
    {
        optionCount = optionList.Count;

        Debug.Log("끝???");
        for (int i = 0; i < optionBtnList.Count; i++)
        {
            if (i < optionCount)
            {
                optionBtnList[i].gameObject.SetActive(true);
                optionBtnList[i].onClick.RemoveAllListeners();
                optionBtnList[i].transform.GetComponent<TextMeshProUGUI>().text = optionList[i];
                optionBtnList[i].onClick.AddListener(() =>
                {
                    // DialogManager의 메서드를 호출하고 선택된 버튼의 인덱스(buttonIndex)를 전달
                    dialogManager.OnOptionSelected(optionIdxList[i]);
                });
                optionBtnList[i].gameObject.SetActive(false);
            }
            else
                optionBtnList[i].gameObject.SetActive(false);

            Debug.Log($"끝??{i}");
        }
    }

    public void StartDialog(string message)
    {
        // 타이핑 이펙트로 보여주고 행동반환
        typer.Typing(dialogMessage, message, onCompleted: () =>
        {
            dialogManager.FinishDialog();
        });
    }

    public void ShowAllOptionBtn()
    {
        for(int i=0; i<optionCount; i++)
        {
            optionBtnList[i].gameObject.SetActive(true);
        }
    }
}
