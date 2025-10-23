using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [Header("UI Object")]
    [SerializeField] private TMP_InputField inputField;

    private bool isChatting = false;

    public void Update()
    {
        CheckInputChat();
    }

    private void CheckInputChat()
    {
        if (inputField == null || inputField.text.Length == 0) return;

        // 채팅 입력하고 엔터 키 입력 받았는 지 체크
        if(Input.GetKeyDown(KeyCode.Return))
        {
            string message = inputField.text;
            Debug.Log(message);
            GameManager.Instance.player.SendMessageOnSpeechBubble(message);
            inputField.text = string.Empty;
        }
    }

    public void OnClickChatButton()
    {
        isChatting = !isChatting;
        this.gameObject.SetActive(isChatting);
    }
}
