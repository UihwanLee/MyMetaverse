using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingOption : MonoBehaviour
{
    public CustomizingOptionData Data { get; set; }

    [SerializeField] private Image optionImg;
    [SerializeField] private TextMeshProUGUI optionName;

    private Color whiteColor;
    private Color darkGrayColor;

    private void Start()
    {
        whiteColor = ColorData.GetColor(EColor.White);
        darkGrayColor = ColorData.GetColor(EColor.DarkGray);
    }

    public void InitOption(CustomizingOptionData optionData)
    {
        Data = optionData;
        optionName.text = optionData.optionName;
        ChangeColorOrigin();
    }

    public void ChangeColorOrigin()
    {
        optionImg.color = whiteColor;
    }

    public void ChangeColorHighlight()
    {
        optionImg.color = darkGrayColor;
    }
}
