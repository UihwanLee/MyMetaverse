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

    #region Ŀ���͸���¡ �ʱ�ȭ

    /// <summary>
    /// 12���� ���� ���� ����
    /// </summary>
    public void GenerateCustomizeSlot()
    {
        // 12���� �÷� �Ӽ� ����
        for (int i = 0; i < colorCount; i++)
        {
            var customizingSlot = Instantiate(customizingSlotPrefab, parent).GetComponent<CustomizingSlot>();

            int idx = i;
            Sprite iconSprite = customizingOptionDataList[0].sprite;
            Color iconColor = ColorData.GetColor(idx);

            // CustomizingSlot �ʱ�ȭ
            customizingSlot.InitSlot(idx, iconSprite, iconColor);

            // Button onClickEvent ����
            Button btn = customizingSlot.GetComponent<Button>();
            CustomizingSlot slot = customizingSlot;
            btn.onClick.AddListener(() => ChooseColor(slot));

            customizingSlotList.Add(customizingSlot);
        }
    }

    /// <summary>
    /// �ɼ� ����
    /// </summary>
    private void GenerateOption()
    {
        for(int i=0; i< customizingOptionDataList.Count; i++)
        {
            int optionIndex = i;
            CustomizingOption customizingOption = Instantiate(customizingOptionPrefab, optionButtonParent).GetComponent<CustomizingOption>();

            // �ɼ� �ʱ�ȭ
            CustomizingOptionData optionData = customizingOptionDataList[i];
            customizingOption.InitOption(optionData);

            // �ɼ� ��ư onClick Event ����
            Button btn = customizingOption.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                ChoiceCustomizingOption(optionIndex);
            });

            if (customizingOption != null) customizingOptionList.Add(customizingOption);
        }
    }

    /// <summary>
    /// Customzing UI�� ų �� ȣ��
    /// Player ���� ������ ���� ���� �ʱ�ȭ
    /// </summary>
    public void InitCustomizing()
    {
        // ���� Player �� ���� �����ϰ� Avatar ���� ����
        Material playerMat = player.GetComponentInChildren<SpriteRenderer>().material;
        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        foreach (var part in customizingOptionDataList)
        {
            GetColorByPart(playerMat, part.materialName, ref part.originColor);
            SetColorByPart(avatarMat, part.materialName, part.originColor);

            // ���� ���� �ʱ�ȭ
            part.selectSlotIdx = NO_SLOT_SELECTED;
            part.banSlotIdx = NO_SLOT_SELECTED;
        }

        SetCustomizingBanSlotByPlayerCurrentColor();
        ChoiceCustomizingOption(0);
        UpdatePurchaseSlot();
    }

    /// <summary>
    /// ���� �� Player ��ġ ���� Ban Idx ����
    /// </summary>
    private void SetCustomizingBanSlotByPlayerCurrentColor()
    {
        // �ɼ� ���� Player ���� ����� ��ġ�ϴ� ���� ������ Ban Slot Idx�� ����

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

    #region Ŀ���͸���¡ �ɼ� Ŭ��

    /// <summary>
    /// Ŀ���͸���¡ �ɼ� ����
    /// </summary>
    public void ChoiceCustomizingOption(int option)
    {
        // Option�� null�̸� �ʱ�ȭ
        if(currentOption == null) currentOption = customizingOptionList[option];

        // ���� OptionBtn ���� ����
        currentOption.ChangeColorOrigin();

        // OptionBtn ���� ����
        currentOption = customizingOptionList[option];
        currentOption.ChangeColorHighlight();

        ChangeCustomizingSlotImage();
        UpdateSlotsByClickOption(currentOption.Data.selectSlotIdx);
        BanCustomizingSlotByCurrentPlayerColor();
    }

    /// <summary>
    /// �ɼ� Ŭ�� �� Slot ����
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
    /// Ŀ���͸���¡ �ɼǿ� ���� ���� �̹��� ����
    /// </summary>
    private void ChangeCustomizingSlotImage()
    {
        for (int i = 0; i < customizingSlotList.Count; i++)
        {
            customizingSlotList[i].SetSpriteSlotIcon(currentOption.Data.sprite);
        }
    }

    /// <summary>
    /// ���� �� Player ��ġ ���� Ban 
    /// </summary>
    private void BanCustomizingSlotByCurrentPlayerColor()
    {
        // �ɼ� �� Ban���� ������ idx�� ã�Ƽ� Ban
        int banIdx = currentOption.Data.banSlotIdx;

        if (banIdx == NO_SLOT_SELECTED) return;

        customizingSlotList[banIdx].BanSlot();
    }

    #endregion

    #region Ŀ���͸���¡ ���� Ŭ��

    /// <summary>
    /// ������ Ŭ���Ͽ��� �� �ش� ���� �ݿ�
    /// </summary>
    public void ChooseColor(CustomizingSlot slot)
    {
        Material avatarMat = avatar.transform.GetComponent<Image>().material;

        // �Ȱ��� ���� ������� Ȯ��
        if (slot.Idx == currentOption.Data.selectSlotIdx)
        {
            ChooseSameColorSlot(avatarMat);
            return;
        }

        ChooseDifferentColorSlot(avatarMat, slot);
    }

    // ������ �Ȱ��� ���� ������ ����
    private void ChooseSameColorSlot(Material avatarMat)
    {
        // �Ȱ��� ���� �����ϸ� ���� �����ϰ� ���� �������� ���ư���
        currentOption.Data.selectSlotIdx = NO_SLOT_SELECTED;
        avatarMat.SetColor(currentOption.Data.materialName, currentOption.Data.originColor);
        UpdateSlotsByClickSlot(NO_SLOT_SELECTED);

        UpdatePurchaseSlot();
    }

    // ������ �ٸ� ���� ���� ����
    private void ChooseDifferentColorSlot(Material avatarMat, CustomizingSlot slot)
    {
        // Highlight �߰�
        UpdateSlotsByClickSlot(slot.Idx);

        //// �� �������� �ƹ�Ÿ ���� ����
        Color changeColor = ColorData.GetColor(slot.Idx);
        avatarMat.SetColor(currentOption.Data.materialName, changeColor);

        // ���� ������ Slot ���� Option ������ ����
        currentOption.Data.selectSlotIdx = slot.Idx;
        currentOption.Data.changeColor = changeColor;

        UpdatePurchaseSlot();
    }

    /// <summary>
    /// ���� Ŭ�� �� ���̶���Ʈ ȿ�� �ݿ�
    /// </summary>
    private void UpdateSlotsByClickSlot(int idx)
    {
        // ���� �����ߴ� ������ ����
        if (currentSlot != null)
            currentSlot.ResetSlot();

        if (idx > customizingSlotList.Count || idx < 0) return;

        // ���� Ŭ���� ���� ���̶���Ʈ
        currentSlot = customizingSlotList[idx];
        currentSlot.HighlightSlot();
    }


    #endregion

    #region Ŀ���͸���¡ ���� ����

    /// <summary>
    /// ���� �õ�
    /// </summary>
    public void TryPurchase()
    {
        int totalPirce = int.Parse(totalPriceTxt.text);

        // ���� ����� �����ϴٸ� NoticeUI ����
        int playerCoin = GameManager.Instance.Player.CurrentCoin;
        if (totalPirce > playerCoin)
        {
            NoticeUI.Instacne.Notice("������ �����մϴ�!");
            return;
        }

        Purchase(totalPirce);
    }

    /// <summary>
    /// ���� Ȯ��
    /// </summary>
    public void Purchase(int totalPrice)
    {
        GameManager.Instance.Player.CurrentCoin -= totalPrice;

        Material playerMat = player.GetComponentInChildren<SpriteRenderer>().material;
        foreach (var part in customizingOptionDataList)
        {
            // ������ ������ �ִٸ� �ش� �������� ����
            if (part.selectSlotIdx != NO_SLOT_SELECTED) { SetColorByPart(playerMat, part.materialName, part.changeColor); }
        }

        CloseCustomizingUI();
    }

    /// <summary>
    /// ���� �׸� ����
    /// </summary>
    private void UpdatePurchaseSlot()
    {
        // purchaseSlotParent�� ��ġ �� �ִ� �ڽ� ������Ʈ ����
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
                // purchaseSlotParent�� purchaseSlotPrefab ����
                var purchaseSlot = Instantiate(purchaseSlotPrefab, purchaseSlotParent);
                purchaseSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Ŀ���͸���¡ - {part.name}";
                purchaseSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{part.price}G";
                totalPrice += part.price;
            }
        }

        totalPriceTxt.text = totalPrice.ToString();
    }

    #endregion

    #region Ŀ���͸���¡ ��ư ���

    /// <summary>
    /// ���� �������� �ǵ�����
    /// </summary>
    public void Revart()
    {
        // ���� �������� ��ü �ǵ�����
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
    /// Ŀ���͸���¡ UI ����
    /// </summary>
    public void OpenCustomizingUI()
    {
        customizingUI.SetActive(true);
    }

    /// <summary>
    /// Ŀ���͸���¡ UI �ݱ�
    /// </summary>
    public void CloseCustomizingUI()
    {
        GameManager.Instance.ChangeGameState(GameState.Playing);
        customizingUI.SetActive(false);
    }

    #endregion

    #region ���� �޼���

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
