using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteract : MonoBehaviour
{
    [SerializeField] private EColor origin;
    [SerializeField] private EColor highlight;

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

        Color newColor = (onClick) ? ColorData.GetColor(highlight) : ColorData.GetColor(origin);
        Image img = GetComponent<Image>();
        img.color = newColor;
    }
}
