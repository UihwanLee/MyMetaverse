using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomizingOption", menuName = "Option/customizingOption")]
public class CustomizingOptionData : ScriptableObject
{
    // 커스터마이징 옵션(부위)에 따라 저장되는 데이터

    public ECustomizingOptionType type;     // 옵션 타입
    public string optionName;               // 옵션 이름
    public Sprite sprite;                   // 커스터마이징 이미지
    public string materialName;             // 적용 마테리얼 Name
    public int selectSlotIdx;               // 현재 선택된 slot idx
    public Color originColor;               // Player 기존 색상
    public Color changeColor;               // 바꾼 색상
    public int banSlotIdx;                  // Ban 슬롯 
    public int price;                       // 커스터마이징 비용
}
