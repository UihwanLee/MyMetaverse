using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeManager : MonoBehaviour
{
    [Header("Transform Info")]
    [SerializeField] private Transform parent;

    [Header("Color Slot")]
    [SerializeField] private GameObject colorBtnPrefab;

    private int colorCount;

    private void Start()
    {
        colorCount = PlayerColor.GetColorCount();
        SetCustomizeColor();
    }

    public void SetCustomizeColor()
    {
        // 12가지 컬러 속성 지정
        for(int i=0; i<colorCount; i++)
        {
            var colorObj = Instantiate(colorBtnPrefab, parent);
            Image colorImage = colorObj.GetComponent<Image>();

            Material m_color = new Material(colorImage.material);
            m_color.color = PlayerColor.GetColor(i);
            colorImage.material = m_color;
        }
    }
}
