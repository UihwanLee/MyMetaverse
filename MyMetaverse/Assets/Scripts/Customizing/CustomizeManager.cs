using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public enum ECustomizingOptionType
{
    CapOption,
    ClothesOption,
    EyeOption
}

public static class MaterialColorName
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
    [SerializeField] private List<CustomizingSlot> customizingSlotList;
    [SerializeField] private GameObject customizingSlotPrefab;

    [Header("Customizing Button UI")]
    [SerializeField] private Button capBtn;
    [SerializeField] private Button clothesBtn;
    [SerializeField] private Button eyeBtn;
    [SerializeField] private List<Sprite> typeSpriteList;

    [Header("Avatar")]
    [SerializeField] private GameObject avatar;

    private int colorCount;
    private PlayerController player;

    private ECustomizingOptionType currentOptionType;
    private string currentOption;

    private void Start()
    {
        customizingSlotList.Clear();
        colorCount = 12;

        currentOption = MaterialColorName.CapColor;
        player = GameManager.Instance.Player;

        SetCustomizeColor();
        ChoiceCustomizingOption(0);
    }

    public void InitAvatarColor()
    {
        // 현재 Player 옷 색상 기준으로 Avatar 색상 변경
        Color capColor = Color.white, clothesColor = Color.white, eyeColor = Color.white;

        Material playerMat = player.GetComponentInChildren<SpriteRenderer>().material;
        GetColorByPart(playerMat, ref capColor, ref clothesColor, ref eyeColor);

        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        SetColorByPart(avatarMat, capColor, clothesColor, eyeColor);
    }

    public void SetCustomizeColor()
    {
        // 12가지 컬러 속성 지정
        for(int i=0; i<colorCount; i++)
        {
            var customizingSlot = Instantiate(customizingSlotPrefab, parent).GetComponent<CustomizingSlot>();

            Button btn = customizingSlot.GetComponent<Button>();
            Image colorImage = customizingSlot.transform.GetChild(0).GetComponent<Image>();
            Material colorMat = new Material(colorImage.material);

            customizingSlot.Idx = i;
            colorImage.sprite = typeSpriteList[0];
            colorImage.material = colorMat;
            colorMat.color = ColorData.GetColor(i);

            CustomizingSlot currentSlot = customizingSlot;
            btn.onClick.AddListener(() => ChooseColor(currentSlot));

            Debug.Log(customizingSlot.Idx);

            customizingSlotList.Add(customizingSlot);
        }
    }

    public void ChoiceCustomizingOption(int option)
    {
        // 커스터마이징 옵션 선택 : Cap, Clothes, Eye

        ECustomizingOptionType type = (ECustomizingOptionType)option;

        capBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.White);
        clothesBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.White);
        eyeBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.White);

        currentOptionType = type;
        switch (type)
        {
            case ECustomizingOptionType.CapOption:
                currentOption = MaterialColorName.CapColor;
                capBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
                break;
            case ECustomizingOptionType.ClothesOption:
                currentOption = MaterialColorName.ClothesColor;
                clothesBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
                break;
            case ECustomizingOptionType.EyeOption:
                currentOption = MaterialColorName.EyeColor;
                eyeBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
                break;
            default:
                break;
        }

        ChangeCustomizingSlotImage();
    }

    private void ChangeCustomizingSlotImage()
    {
        int index = (int)currentOptionType;
        for(int i=0; i<customizingSlotList.Count; i++)
        {
            Image img = customizingSlotList[i].transform.GetChild(0).GetComponent<Image>();
            img.sprite = typeSpriteList[index];
        }
    }

    public void ChooseColor(CustomizingSlot slot)
    {
        //// 고른 색상으로 아바타 색상 변경
        Material avatarMat = avatar.transform.GetComponent<Image>().material;

        //Debug.Log(slot.Idx.ToString() + ColorData.GetColor(slot.Idx));
        if (avatarMat) avatarMat.SetColor(currentOption, ColorData.GetColor(slot.Idx));
    }

    public void Confirm()
    {
        // 최종 결정
        Color capColor = Color.white, clothesColor = Color.white, eyeColor = Color.white;

        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        GetColorByPart(avatarMat, ref capColor, ref clothesColor, ref eyeColor);

        Material playerMat = player.GetComponentInChildren<SpriteRenderer>().material;
        SetColorByPart(playerMat, capColor, clothesColor, eyeColor);
    }

    private void GetColorByPart(Material target, ref Color capColor, ref Color clothesColor, ref Color eyeColor)
    {
        capColor = target.GetColor(MaterialColorName.CapColor);
        clothesColor = target.GetColor(MaterialColorName.ClothesColor);
        eyeColor = target.GetColor(MaterialColorName.EyeColor);
    }

    private void SetColorByPart(Material target, Color capColor, Color clothesColor, Color eyeColor)
    {
        target.SetColor(MaterialColorName.CapColor, capColor);
        target.SetColor(MaterialColorName.ClothesColor, clothesColor);
        target.SetColor(MaterialColorName.EyeColor, eyeColor);
    }
}
