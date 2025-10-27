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

    [Header("Color Slot")]
    [SerializeField] private List<CustomizingSlot> customizingSlotList;
    [SerializeField] private GameObject customizingSlotPrefab;

    [Header("Customizing Option")]
    [SerializeField] private List<CustomizingOptionData> customizingOptionDataList;
    [SerializeField] private CustomizingOptionData currentOption;

    [Header("Customizing Button UI")]
    [SerializeField] private Transform optionButtonParent;
    [SerializeField] private GameObject customizingOptionPrefab;
    [SerializeField] private List<Button> customizingOptionList;
    [SerializeField] private GameObject customizingUI;
    private Button currentOptionBtn;

    [Header("Purchase")]
    [SerializeField] private Transform purchaseSlotParent;
    [SerializeField] private GameObject purchaseSlotPrefab;
    [SerializeField] private TextMeshProUGUI totalPriceTxt;

    [Header("Avatar")]
    [SerializeField] private GameObject avatar;

    private int colorCount;
    private PlayerController player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        customizingSlotList.Clear();
        colorCount = 12;

        GenerateCustomizeSlot();
        GenerateOptionBtn();

        customizingUI.SetActive(false);
    }

    public void OpenCustomizingUI()
    {
        customizingUI.SetActive(true);
    }

    #region �ʱ�ȭ

    /// <summary>
    /// 12���� ���� ���� ����
    /// </summary>
    public void GenerateCustomizeSlot()
    {
        // 12���� �÷� �Ӽ� ����
        for (int i = 0; i < colorCount; i++)
        {
            var customizingSlot = Instantiate(customizingSlotPrefab, parent).GetComponent<CustomizingSlot>();

            Button btn = customizingSlot.GetComponent<Button>();
            Image colorImage = customizingSlot.transform.GetChild(2).GetComponent<Image>();
            Material colorMat = new Material(colorImage.material);

            customizingSlot.Idx = i;
            colorImage.sprite = customizingOptionDataList[0].sprite;
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
    /// �ɼ� ��ư ����
    /// </summary>
    private void GenerateOptionBtn()
    {
        for(int i=0; i< customizingOptionDataList.Count; i++)
        {
            int optionIndex = i;
            Button customizingOptionBtn = Instantiate(customizingOptionPrefab, optionButtonParent).GetComponent<Button>();
            customizingOptionBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.White);
            customizingOptionBtn.GetComponentInChildren<TextMeshProUGUI>().text = customizingOptionDataList[i].optionName;
            customizingOptionBtn.onClick.AddListener(() =>
            {
                ChoiceCustomizingOption(optionIndex);
            });

            if (customizingOptionBtn != null) customizingOptionList.Add(customizingOptionBtn);
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
            part.selectSlotIdx = -1;
            part.banSlotIdx = -1;
        }

        SetCustomizingBanSlotByPlayerCurrentColor();
        ChoiceCustomizingOption(0);
        UpdatePurchaseSlot();
    }

    #endregion

    #region Ŀ���͸���¡ System

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

    /// <summary>
    /// Ŀ���͸���¡ �ɼ� ����
    /// </summary>
    public void ChoiceCustomizingOption(int option)
    {
        // Ŀ���͸���¡ �ɼ� ���� 
        currentOption = customizingOptionDataList[option];

        // OptionBtn�� ������ �ʱ�ȭ
        if (currentOptionBtn == null) currentOptionBtn = customizingOptionList[option];

        // ���� OptionBtn ���� ����
        currentOptionBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.White);

        // OptionBtn ���� ����
        currentOptionBtn = customizingOptionList[option];
        currentOptionBtn.GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);

        ChangeCustomizingSlotImage();
        UpdateSlotsByClickSlot(currentOption.selectSlotIdx);
        BanCustomizingSlotByCurrentPlayerColor();
    }

    /// <summary>
    /// Ŀ���͸���¡ �ɼǿ� ���� ���� �̹��� ����
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
    /// ���� �� Player ��ġ ���� Ban 
    /// </summary>
    private void BanCustomizingSlotByCurrentPlayerColor()
    {
        // �ɼ� �� Ban���� ������ idx�� ã�Ƽ� Ban
        int banIdx = currentOption.banSlotIdx;

        if (banIdx == -1) return;

        customizingSlotList[banIdx].GetComponent<Button>().interactable = false;
        customizingSlotList[banIdx].transform.GetChild(1).GetComponent<Image>().color = ColorData.GetColor(EColor.DarkGray);
        customizingSlotList[banIdx].transform.GetChild(3).gameObject.SetActive(true);
    }

    /// <summary>
    /// ���� �������� �ǵ�����
    /// </summary>
    public void Revart()
    {
        // ���� �������� ��ü �ǵ�����
        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        foreach (var part in customizingOptionDataList)
        {
            part.selectSlotIdx = -1;
            SetColorByPart(avatarMat, part.materialName, part.originColor);
        }

        UpdatePurchaseSlot();
        UpdateSlotsByClickSlot(-1);
    }

    #endregion

    #region ���� ����

    /// <summary>
    /// ������ Ŭ���Ͽ��� �� �ش� ���� �ݿ�
    /// </summary>
    public void ChooseColor(CustomizingSlot slot)
    {
        Material avatarMat = avatar.transform.GetComponent<Image>().material;

        // �Ȱ��� ���� ������� Ȯ��
        if (slot.Idx == currentOption.selectSlotIdx)
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
        currentOption.selectSlotIdx = -1;
        avatarMat.SetColor(currentOption.materialName, currentOption.originColor);
        UpdateSlotsByClickSlot(-1);

        UpdatePurchaseSlot();
    }

    // ������ �ٸ� ���� ���� ����
    private void ChooseDifferentColorSlot(Material avatarMat, CustomizingSlot slot)
    {
        // Highlight �߰�
        UpdateSlotsByClickSlot(slot.Idx);

        //// �� �������� �ƹ�Ÿ ���� ����
        Color changeColor = ColorData.GetColor(slot.Idx);
        avatarMat.SetColor(currentOption.materialName, changeColor);

        // ���� ������ Slot ���� Option ������ ����
        currentOption.selectSlotIdx = slot.Idx;
        currentOption.changeColor = changeColor;

        UpdatePurchaseSlot();
    }

    #endregion

    #region UI ������Ʈ

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
            if(part.selectSlotIdx != -1)
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

    /// <summary>
    /// ���� Ŭ�� �� ���̶���Ʈ ȿ�� �ݿ�
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

    #endregion

    #region ����

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
            if (part.selectSlotIdx != -1) { SetColorByPart(playerMat, part.materialName, part.changeColor); }
        }

        CloseCustomizingUI();
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
