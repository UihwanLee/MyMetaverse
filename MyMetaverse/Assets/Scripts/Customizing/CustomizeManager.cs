using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private static CustomizeManager instance;
    public static CustomizeManager Instance {  get { return instance; } }

    [Header("Transform Info")]
    [SerializeField] private Transform parent;

    [Header("customizing Slot")]
    [SerializeField] private List<CustomizingSlot> customizingSlotList;
    [SerializeField] private GameObject customizingSlotPrefab;
    private CustomizingSlot currentSlot;

    [Header("Customizing Option")]
    [SerializeField] private List<CustomizingOptionData> customizingOptionDataList;

    [Header("Customizing Button UI")]
    [SerializeField] private Transform optionButtonParent;
    [SerializeField] private GameObject customizingOptionPrefab;
    [SerializeField] private List<CustomizingOption> customizingOptionList;
    [SerializeField] private GameObject customizingUI;
    private CustomizingOption currentOption;

    [Header("Purchase")]
    [SerializeField] private Transform purchaseSlotParent;
    [SerializeField] private GameObject purchaseSlotPrefab;
    [SerializeField] private TextMeshProUGUI totalPriceTxt;

    [Header("Avatar")]
    [SerializeField] private GameObject avatar;

    private int colorCount;
    private PlayerController player;

    private Color whiteColor;
    private Color darkGrayColor;

    public const int SLOT_COUNT = 12;
    public const int NO_SLOT_SELECTED = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        customizingSlotList.Clear();
        colorCount = SLOT_COUNT;

        GenerateOption();
        GenerateCustomizeSlot();

        customizingUI.SetActive(false);

        whiteColor = ColorData.GetColor(EColor.White);
        darkGrayColor = ColorData.GetColor(EColor.DarkGray);
    }

    #region 커스터마이징 초기화

    /// <summary>
    /// 12가지 색상 슬롯 생성
    /// </summary>
    public void GenerateCustomizeSlot()
    {
        // 12가지 컬러 속성 지정
        for (int i = 0; i < colorCount; i++)
        {
            var customizingSlot = Instantiate(customizingSlotPrefab, parent).GetComponent<CustomizingSlot>();

            int idx = i;
            Sprite iconSprite = customizingOptionDataList[0].sprite;
            Color iconColor = ColorData.GetColor(idx);

            // CustomizingSlot 초기화
            customizingSlot.InitSlot(idx, iconSprite, iconColor);

            // Button onClickEvent 설정
            Button btn = customizingSlot.GetComponent<Button>();
            CustomizingSlot slot = customizingSlot;
            btn.onClick.AddListener(() => ChooseColor(slot));

            customizingSlotList.Add(customizingSlot);
        }
    }

    /// <summary>
    /// 옵션 생성
    /// </summary>
    private void GenerateOption()
    {
        for(int i=0; i< customizingOptionDataList.Count; i++)
        {
            int optionIndex = i;
            CustomizingOption customizingOption = Instantiate(customizingOptionPrefab, optionButtonParent).GetComponent<CustomizingOption>();

            // 옵션 초기화
            CustomizingOptionData optionData = customizingOptionDataList[i];
            customizingOption.InitOption(optionData);

            // 옵션 버튼 onClick Event 설정
            Button btn = customizingOption.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                ChoiceCustomizingOption(optionIndex);
            });

            if (customizingOption != null) customizingOptionList.Add(customizingOption);
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
            part.selectSlotIdx = NO_SLOT_SELECTED;
            part.banSlotIdx = NO_SLOT_SELECTED;
        }

        SetCustomizingBanSlotByPlayerCurrentColor();
        ChoiceCustomizingOption(0);
        UpdatePurchaseSlot();
    }

    /// <summary>
    /// 부위 별 Player 일치 슬롯 Ban Idx 저장
    /// </summary>
    private void SetCustomizingBanSlotByPlayerCurrentColor()
    {
        // 옵션 별로 Player 현재 색상과 일치하는 색상 슬롯은 Ban Slot Idx에 저장

        foreach (var part in customizingOptionDataList)
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

    #region 커스터마이징 옵션 클릭

    /// <summary>
    /// 커스터마이징 옵션 선택
    /// </summary>
    public void ChoiceCustomizingOption(int option)
    {
        // Option이 null이면 초기화
        if(currentOption == null) currentOption = customizingOptionList[option];

        // 기존 OptionBtn 색상 변경
        currentOption.ChangeColorOrigin();

        // OptionBtn 색상 변경
        currentOption = customizingOptionList[option];
        currentOption.ChangeColorHighlight();

        ChangeCustomizingSlotImage();
        UpdateSlotsByClickOption(currentOption.Data.selectSlotIdx);
        BanCustomizingSlotByCurrentPlayerColor();
    }

    /// <summary>
    /// 옵션 클릭 시 Slot 변경
    /// </summary>
    private void UpdateSlotsByClickOption(int idx)
    {
        for (int i = 0; i < customizingSlotList.Count; i++)
        {
            customizingSlotList[i].ResetSlot();
        }

        if (idx > customizingSlotList.Count || idx < 0) return;

        customizingSlotList[idx].HighlightSlot();
    }

    /// <summary>
    /// 커스터마이징 옵션에 따른 슬롯 이미지 변경
    /// </summary>
    private void ChangeCustomizingSlotImage()
    {
        for (int i = 0; i < customizingSlotList.Count; i++)
        {
            customizingSlotList[i].SetSpriteSlotIcon(currentOption.Data.sprite);
        }
    }

    /// <summary>
    /// 부위 별 Player 일치 슬롯 Ban 
    /// </summary>
    private void BanCustomizingSlotByCurrentPlayerColor()
    {
        // 옵션 별 Ban으로 지정된 idx를 찾아서 Ban
        int banIdx = currentOption.Data.banSlotIdx;

        if (banIdx == NO_SLOT_SELECTED) return;

        customizingSlotList[banIdx].BanSlot();
    }

    #endregion

    #region 커스터마이징 슬롯 클릭

    /// <summary>
    /// 슬롯을 클릭하였을 때 해당 색상 반영
    /// </summary>
    public void ChooseColor(CustomizingSlot slot)
    {
        Material avatarMat = avatar.transform.GetComponent<Image>().material;

        // 똑같은 슬롯 골랐는지 확인
        if (slot.Idx == currentOption.Data.selectSlotIdx)
        {
            ChooseSameColorSlot(avatarMat);
            return;
        }

        ChooseDifferentColorSlot(avatarMat, slot);
    }

    // 기존에 똑같은 색상 슬롯을 고를시
    private void ChooseSameColorSlot(Material avatarMat)
    {
        // 똑같은 슬롯 설정하면 선택 해제하고 기존 색상으로 돌아가기
        currentOption.Data.selectSlotIdx = NO_SLOT_SELECTED;
        avatarMat.SetColor(currentOption.Data.materialName, currentOption.Data.originColor);
        UpdateSlotsByClickSlot(NO_SLOT_SELECTED);

        UpdatePurchaseSlot();
    }

    // 기존과 다른 색상 슬롯 고를시
    private void ChooseDifferentColorSlot(Material avatarMat, CustomizingSlot slot)
    {
        // Highlight 추가
        UpdateSlotsByClickSlot(slot.Idx);

        //// 고른 색상으로 아바타 색상 변경
        Color changeColor = ColorData.GetColor(slot.Idx);
        avatarMat.SetColor(currentOption.Data.materialName, changeColor);

        // 현재 선택한 Slot 정보 Option 정보에 저장
        currentOption.Data.selectSlotIdx = slot.Idx;
        currentOption.Data.changeColor = changeColor;

        UpdatePurchaseSlot();
    }

    /// <summary>
    /// 슬롯 클릭 시 하이라이트 효과 반영
    /// </summary>
    private void UpdateSlotsByClickSlot(int idx)
    {
        // 전에 선택했던 슬롯은 리셋
        if (currentSlot != null)
            currentSlot.ResetSlot();

        if (idx > customizingSlotList.Count || idx < 0) return;

        // 새로 클릭한 슬롯 하이라이트
        currentSlot = customizingSlotList[idx];
        currentSlot.HighlightSlot();
    }


    #endregion

    #region 커스터마이징 색상 구매

    /// <summary>
    /// 구매 시도
    /// </summary>
    public void TryPurchase()
    {
        int totalPirce = int.Parse(totalPriceTxt.text);

        // 구매 비용이 부족하다면 NoticeUI 띄우기
        int playerCoin = GameManager.Instance.Player.CurrentCoin;
        if (totalPirce > playerCoin)
        {
            NoticeUI.Instacne.Notice("코인이 부족합니다!");
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
            if (part.selectSlotIdx != NO_SLOT_SELECTED) { SetColorByPart(playerMat, part.materialName, part.changeColor); }
        }

        CloseCustomizingUI();
    }

    /// <summary>
    /// 구매 항목 수정
    /// </summary>
    private void UpdatePurchaseSlot()
    {
        // purchaseSlotParent에 위치 해 있는 자식 오브젝트 삭제
        int childCount = purchaseSlotParent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(purchaseSlotParent.GetChild(i).gameObject);
        }

        int totalPrice = 0;

        foreach (var part in customizingOptionDataList)
        {
            if (part.selectSlotIdx != NO_SLOT_SELECTED)
            {
                // purchaseSlotParent에 purchaseSlotPrefab 생성
                var purchaseSlot = Instantiate(purchaseSlotPrefab, purchaseSlotParent);
                purchaseSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"커스터마이징 - {part.name}";
                purchaseSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{part.price}G";
                totalPrice += part.price;
            }
        }

        totalPriceTxt.text = totalPrice.ToString();
    }

    #endregion

    #region 커스터마이징 버튼 기능

    /// <summary>
    /// 원래 색상으로 되돌리기
    /// </summary>
    public void Revart()
    {
        // 원래 색상으로 전체 되돌리기
        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        foreach (var part in customizingOptionDataList)
        {
            part.selectSlotIdx = NO_SLOT_SELECTED;
            SetColorByPart(avatarMat, part.materialName, part.originColor);
        }

        UpdatePurchaseSlot();
        UpdateSlotsByClickSlot(NO_SLOT_SELECTED);
    }

    /// <summary>
    /// 커스터마이징 UI 열기
    /// </summary>
    public void OpenCustomizingUI()
    {
        customizingUI.SetActive(true);
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
