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
    private PlayerController player;

    private void Start()
    {
        player = GameManager.Instance.player;
        inputField.onSubmit.AddListener(delegate { SendMessageToPlayer(); });
    }

    public void SendMessageToPlayer()
    {
        if (player != null && inputField.text.Length > 0)
        {
            string message = inputField.text;
            player.SendMessageOnSpeechBubble(message);
            inputField.text = string.Empty;
        }
    }

    public void OnClickChatButton()
    {
        isChatting = !isChatting;
        this.gameObject.SetActive(isChatting);
    }
}
