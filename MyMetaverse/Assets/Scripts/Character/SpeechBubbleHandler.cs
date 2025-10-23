using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class SpeechBubbleHandler : MonoBehaviour
{
    [SerializeField] private float duration = 2f;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMeshProUGUI messageUI;
    [SerializeField] private float maxWidth = 5f;

    private Vector2 padding = new Vector2(1f, 1f);

    public void StartSpeech(string message)
    {
        SetSpeechBubbleUIBaseMessage(message);
        StartCoroutine(SpeechCoroutine(message));
    }

    // �ؽ�Ʈ�� �°� UI ������ ����
    private void SetSpeechBubbleUIBaseMessage(string message)
    {
        float fixedWidth = maxWidth;
        var textRect = messageUI.rectTransform;
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fixedWidth);

        Vector2 preferred = messageUI.GetPreferredValues(message, fixedWidth, 0);

        Vector2 newSize = new Vector2(fixedWidth, preferred.y) + padding;
        speechBubble.GetComponent<RectTransform>().sizeDelta = newSize;
    }

    // ��ǳ�� �ڷ�ƾ
    private IEnumerator SpeechCoroutine(string message)
    {
        this.gameObject.SetActive(true);

        this.messageUI.text = message;

        yield return new WaitForSeconds(duration);

        this.gameObject.SetActive(false);

        yield return null;
    }
}
