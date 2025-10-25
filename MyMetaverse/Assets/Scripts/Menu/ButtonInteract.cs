using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteract : MonoBehaviour
{
    [SerializeField] private Color originColor;
    [SerializeField] private Color pressedColor;

    private bool onClick;

    private void Start()
    {
        onClick = false;
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickEvent);
    }

    public void OnClickEvent()
    {
        onClick = !onClick;

        Debug.Log("Pressed");
        Color color = (onClick) ? pressedColor : originColor;
        Image img = GetComponent<Image>();
        img.color = color;
        Debug.Log(img.color);
    }
}
