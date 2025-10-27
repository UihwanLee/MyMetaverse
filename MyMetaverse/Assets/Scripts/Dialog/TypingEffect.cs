using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.1f;

    private WaitForSeconds tyingDelay;

    private Coroutine typingCoroutine;

    private void Start()
    {
        tyingDelay = new WaitForSeconds(typingSpeed);
    }

    public void Typing(TextMeshProUGUI textUI, string message, Action onCompleted)
    {
        StartCoroutine(TypingCoroutine(textUI, message, onCompleted));
    }

    private IEnumerator TypingCoroutine(TextMeshProUGUI textUI, string message, Action onCompleted)
    {
        // �ʹ� text UI ����
        textUI.text = string.Empty;

        foreach(char c in message)
        {
            textUI.text += c;

            yield return tyingDelay;
        }

        // Typing �����ٰ� Action ��������Ʈ�� �˸�
        onCompleted?.Invoke();
    }
}
