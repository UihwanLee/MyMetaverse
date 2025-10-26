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

    [Header("Customizing Option")]
    [SerializeField] private List<CustomizingOptionData> customizingOptionDataList;
    [SerializeField] private CustomizingOptionData currentOption;

    [Header("Customizing Button UI")]
    [SerializeField] private Button capBtn;
    [SerializeField] private Button clothesBtn;
    [SerializeField] private Button eyeBtn;
    [SerializeField] private List<Sprite> typeSpriteList;
    [SerializeField] private GameObject customizingUI;

    [Header("Avatar")]
    [SerializeField] private GameObject avatar;

    private int colorCount;
    private PlayerController player;

    private void Start()
    {
        player = GameManager.Instance.Player;
        customizingSlotList.Clear();
        colorCount = 12;

        GenerateCustomizeSlot();

        customizingUI.SetActive(false);
    }

    #region 초기화

    /// <summary>
    /// 12가지 색상 슬롯 생성
    /// </summary>
    public void GenerateCustomizeSlot()
    {
        // 12가지 컬러 속성 지정
        for (int i = 0; i < colorCount; i++)
        {
            var customizingSlot = Instantiate(customizingSlotPrefab, parent).GetComponent<CustomizingSlot>();

            Button btn = customizingSlot.GetComponent<Button>();
            Image colorImage = customizingSlot.transform.GetChild(2).GetComponent<Image>();
            Material colorMat = new Material(colorImage.material);

            customizingSlot.Idx = i;
            colorImage.sprite = typeSpriteList[0];
            colorImage.material = colorMat;
            colorMat.color = ColorData.GetColor(i);
            customizingSlot.transform.GetChild(0).gameObject.SetActive(false);
            customizingSlot.transform.GetChild(3).gameObject.SetActive(false);
            customizingSlot.transform.GetChild(1).GetComponent<Image>().color = ColorData.GetColor(EColor.White);

            CustomizingSlot currentSlot = customizingSlot;
            btn.onClick.AddListener(() => ChooseColor(currentSlot));

            customizingSlotList.Add(customizingSlot);
        }
    }

    /// <summary>
    /// Customzing UI를 킬 때 호출
    /// Player 색상 정보와 슬롯 정보 초기화
    /// </summary>
    public void InitCustomizing()
    {
        // 현재 Player 옷 색상 저장하고 Avatar 색상 변경
        Material playerMat = player.GetComponentInChildren<SpriteRenderer>().material;
        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        foreach (var part in customizingOptionDataList)
        {
            GetColorByPart(playerMat, part.materialName, ref part.originColor);
            SetColorByPart(avatarMat, part.materialName, part.originColor);

            // 슬롯 정보 초기화
            part.selectSlotIdx = -1;
            part.banSlotIdx = -1;
        }

        SetCustomizingBanSlotByPlayerCurrentColor();
        ChoiceCustomizingOption(0);
    }

    /// <summary>
    /// 부위 별 Player 일치 슬롯 Ban Idx 저장
    /// </summary>
    private void SetCustomizingBanSlotByPlayerCurrentColor()
    {
        // 옵션 별로 Player 현재 색상과 일치하는 색상 슬롯은 Ban Slot Idx에 저장

        foreach(var part in customizingOptionDataList)
        {
            for (int i = 0; i < customizingSlotList.Count; i++)
            {
                Color slotColor = customizingSlotList[i].transform.GetChild(2).GetComponent<Image>().material.color;
                if (slotColor.Equals(part.originColor))
                {
                    part.banSlotIdx = i;             
                    break;
                }
            }
        }
    }

    #endregion

    #region 커스터마이징 System

    /// <summary>
    /// 커스터마이징 옵션 선택
    /// </summary>
    public void ChoiceCustomizingOption(int option)
    {
        // 커스터마이징 옵션 선택 : Cap, Clothes, Eye

        capBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.White);
        clothesBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.White);
        eyeBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.White);

        currentOption = customizingOptionDataList[option];
        switch (currentOption.type)
        {
            case ECustomizingOptionType.CapOption:
                capBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
                break;
            case ECustomizingOptionType.ClothesOption:
                clothesBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
                break;
            case ECustomizingOptionType.EyeOption:
                eyeBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
                break;
            default:
                break;
        }

        ChangeCustomizingSlotImage();
        UpdateSlotsByClickSlot(currentOption.selectSlotIdx);
        BanCustomizingSlotByCurrentPlayerColor();
    }

    /// <summary>
    /// 커스터마이징 옵션에 따른 슬롯 이미지 변경
    /// </summary>
    private void ChangeCustomizingSlotImage()
    {
        for (int i = 0; i < customizingSlotList.Count; i++)
        {
            Image img = customizingSlotList[i].transform.GetChild(2).GetComponent<Image>();
            img.sprite = currentOption.sprite;
        }
    }

    /// <summary>
    /// 부위 별 Player 일치 슬롯 Ban 
    /// </summary>
    private void BanCustomizingSlotByCurrentPlayerColor()
    {
        // 옵션 별 Ban으로 지정된 idx를 찾아서 Ban
        int banIdx = currentOption.banSlotIdx;

        if (banIdx == -1) return;

        customizingSlotList[banIdx].GetComponent<Button>().interactable = false;
        customizingSlotList[banIdx].transform.GetChild(1).GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
        customizingSlotList[banIdx].transform.GetChild(3).gameObject.SetActive(true);
    }

    #endregion

    #region UI 버튼 동작

    /// <summary>
    /// 슬롯을 클릭하였을 때 해당 색상 반영
    /// </summary>
    public void ChooseColor(CustomizingSlot slot)
    {
        // Highlight 추가
        UpdateSlotsByClickSlot(slot.Idx);

        //// 고른 색상으로 아바타 색상 변경
        Color changeColor = ColorData.GetColor(slot.Idx);
        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        avatarMat.SetColor(currentOption.materialName, changeColor);

        // 현재 선택한 Slot 정보 Option 정보에 저장
        currentOption.selectSlotIdx = slot.Idx;
        currentOption.changeColor = changeColor;
    }

    /// <summary>
    /// 슬롯 클릭 시 하이라이트 효과 반영
    /// </summary>
    private void UpdateSlotsByClickSlot(int idx)
    {
        for (int i = 0; i < customizingSlotList.Count; i++)
        {
            customizingSlotList[i].transform.GetChild(0).gameObject.SetActive(false);
            customizingSlotList[i].transform.GetChild(1).GetComponent<Image>().color = ColorData.GetColor(EColor.White);
            customizingSlotList[i].transform.GetChild(3).gameObject.SetActive(false);
        }

        if (idx > customizingSlotList.Count || idx < 0) return;

        customizingSlotList[idx].transform.GetChild(0).gameObject.SetActive(true);
        customizingSlotList[idx].transform.GetChild(1).GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
    }

    /// <summary>
    /// 원래 색상으로 되돌리기
    /// </summary>
    public void Revart()
    {
        // 원래 색상으로 전체 되돌리기
        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        foreach(var part in customizingOptionDataList)
        {
            SetColorByPart(avatarMat, part.materialName, part.originColor);
        }
    }

    /// <summary>
    /// 구매 시도
    /// </summary>
    public void TryPurchase()
    {
        int totalPirce = 0;

        foreach(var choice in customizingOptionDataList)
        {
            if(choice.selectSlotIdx != -1)
            {
                // 선택한 항목이 있다면 price 추가
                totalPirce += choice.price;
            }
        }

        // 구매 비용이 부족하다면 NoticeUI 띄우기
        if(totalPirce < GameManager.Instance.Player.CurrentCoin)
        {
            Debug.Log("돈이 부족합니다!");
            return;
        }

        Purchase(totalPirce);
    }

    /// <summary>
    /// 구매 확정
    /// </summary>
    public void Purchase(int totalPrice)
    {
        GameManager.Instance.Player.CurrentCoin -= totalPrice;

        Material playerMat = player.GetComponentInChildren<SpriteRenderer>().material;
        foreach (var part in customizingOptionDataList)
        {
            // 선택한 색상이 있다면 해당 색상으로 변경
            if (part.selectSlotIdx != -1) { SetColorByPart(playerMat, part.materialName, part.changeColor); }
        }

        CloseCustomizingUI();
    }

    /// <summary>
    /// 커스터마이징 UI 닫기
    /// </summary>
    public void CloseCustomizingUI()
    {
        GameManager.Instance.ChangeGameState(GameState.Playing);
        customizingUI.SetActive(false);
    }

    #endregion

    #region 재사용 메서드

    private void GetColorByPart(Material target, string partName, ref Color partColor)
    {
        partColor = target.GetColor(partName);
    }

    private void SetColorByPart(Material target, string partName, Color partColor)
    {
        target.SetColor(partName, partColor);
    }

    #endregion
}
