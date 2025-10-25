using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public static class CustomizingOption
{
    public static readonly string CapColor = "_CapColor";
    public static readonly string ClothesColor = "_ClothesColor";
    public static readonly string EyeColor = "_EyeColor";
}

public class CustomizeManager : MonoBehaviour
{
    [Header("Transform Info")]
    [SerializeField] private Transform parent;

    [Header("Color Slot")]
    [SerializeField] private List<CustomizingSlot> colorSlotList;
    [SerializeField] private GameObject colorBtnPrefab;

    [Header("Avatar")]
    [SerializeField] private GameObject avatar;

    private int colorCount;
    private string currentOption;

    private void Start()
    {
        colorSlotList.Clear();
        colorCount = PlayerColor.GetColorCount();
        SetCustomizeColor();

        currentOption = CustomizingOption.CapColor;
    }

    public void SetCutomizingOption(int option)
    {
        switch (option)
        {
            case 0:
                currentOption = CustomizingOption.CapColor;
                break;
            case 1:
                currentOption = CustomizingOption.ClothesColor;
                break;
            case 2:
                currentOption = CustomizingOption.EyeColor;
                break;
            default:
                break;
        }
    }

    public void SetCustomizeColor()
    {
        // 12���� �÷� �Ӽ� ����
        for(int i=0; i<colorCount; i++)
        {
            var customizingSlot = Instantiate(colorBtnPrefab, parent).GetComponent<CustomizingSlot>();
            customizingSlot.Idx = i;

            Button btn = customizingSlot.GetComponent<Button>();
            Image colorImage = customizingSlot.GetComponent<Image>();
            Material colorMat = new Material(colorImage.material);

            customizingSlot.Idx = i;
            colorImage.material = colorMat;
            colorMat.color = PlayerColor.GetColor(i);
            btn.onClick.AddListener(() => ChooseColor(customizingSlot));

            colorSlotList.Add(customizingSlot);
        }
    }

    public void InitAvatarColor()
    {
        // ���� Player �� ���� �������� Avatar ���� ����
        Color capColor, clothesColor, eyeColor;
        PlayerController player = GameManager.Instance.player;
        if (player)
        {
            Material playerMat = player.GetComponentInChildren<SpriteRenderer>().material;
            capColor = playerMat.GetColor(CustomizingOption.CapColor);
            clothesColor = playerMat.GetColor(CustomizingOption.ClothesColor);
            eyeColor = playerMat.GetColor(CustomizingOption.EyeColor);
            playerMat.SetColor(CustomizingOption.CapColor, capColor);
            playerMat.SetColor(CustomizingOption.ClothesColor, clothesColor);
            playerMat.SetColor(CustomizingOption.EyeColor, eyeColor);

            Material mat = avatar.transform.GetComponent<Image>().material;
            mat.SetColor(CustomizingOption.CapColor, capColor);
            mat.SetColor(CustomizingOption.ClothesColor, clothesColor);
            mat.SetColor(CustomizingOption.EyeColor, eyeColor);
        }
    }

    public void ChooseColor(CustomizingSlot slot)
    {
        // �� �������� �ƹ�Ÿ ���� ����
        Material mat = avatar.transform.GetComponent<Image>().material;
        if (mat) mat.SetColor(currentOption, PlayerColor.GetColor(slot.Idx));
    }

    public void Confirm()
    {
        // ���� ����
        Material mat = avatar.transform.GetComponent<Image>().material;
        Color capColor = mat.GetColor(CustomizingOption.CapColor);
        Color clothesColor = mat.GetColor(CustomizingOption.ClothesColor);
        Color eyeColor = mat.GetColor(CustomizingOption.EyeColor);

        PlayerController player = GameManager.Instance.player;
        if(player)
        {
            Material playerMat = player.GetComponentInChildren<SpriteRenderer>().material;
            playerMat.SetColor(CustomizingOption.CapColor, capColor);
            playerMat.SetColor(CustomizingOption.ClothesColor, clothesColor);
            playerMat.SetColor(CustomizingOption.EyeColor, eyeColor);
        }
    }
}
