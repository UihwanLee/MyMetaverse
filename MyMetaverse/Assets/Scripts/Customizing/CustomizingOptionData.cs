using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomizingOption", menuName = "Option/customizingOption")]
public class CustomizingOptionData : ScriptableObject
{
    // Ŀ���͸���¡ �ɼ�(����)�� ���� ����Ǵ� ������

    public ECustomizingOptionType type;     // �ɼ� Ÿ��
    public string optionName;               // �ɼ� �̸�
    public Sprite sprite;                   // Ŀ���͸���¡ �̹���
    public string materialName;             // ���� ���׸��� Name
    public int selectSlotIdx;               // ���� ���õ� slot idx
    public Color originColor;               // Player ���� ����
    public Color changeColor;               // �ٲ� ����
    public int banSlotIdx;                  // Ban ���� 
    public int price;                       // Ŀ���͸���¡ ���
}
