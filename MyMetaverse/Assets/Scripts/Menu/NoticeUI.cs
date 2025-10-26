using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoticeUI : MonoBehaviour
{
    private static NoticeUI instacne;
    public static NoticeUI Instacne { get { return instacne; } }

    [SerializeField] private GameObject noticeUI;
    [SerializeField] private TextMeshProUGUI noticeTxt;

    private void Awake()
    {
        instacne = this;
    }

    private void Start()
    {
        noticeUI.SetActive(false);
    }

    public void Notice(string message)
    {
        noticeUI.SetActive(true);
        noticeTxt.text = message;
    }

    public void CloseNotice()
    {
        noticeUI.SetActive(false);
    }
}
