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
    }

    /// <summary>
    /// ���� �� Player ��ġ ���� Ban Idx ����
    /// </summary>
    private void SetCustomizingBanSlotByPlayerCurrentColor()
    {
        // �ɼ� ���� Player ���� ����� ��ġ�ϴ� ���� ������ Ban Slot Idx�� ����

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

    #region Ŀ���͸���¡ System

    /// <summary>
    /// Ŀ���͸���¡ �ɼ� ����
    /// </summary>
    public void ChoiceCustomizingOption(int option)
    {
        // Ŀ���͸���¡ �ɼ� ���� : Cap, Clothes, Eye

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

    #endregion

    #region UI ��ư ����

    /// <summary>
    /// ������ Ŭ���Ͽ��� �� �ش� ���� �ݿ�
    /// </summary>
    public void ChooseColor(CustomizingSlot slot)
    {
        // Highlight �߰�
        UpdateSlotsByClickSlot(slot.Idx);

        //// �� �������� �ƹ�Ÿ ���� ����
        Color changeColor = ColorData.GetColor(slot.Idx);
        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        avatarMat.SetColor(currentOption.materialName, changeColor);

        // ���� ������ Slot ���� Option ������ ����
        currentOption.selectSlotIdx = slot.Idx;
        currentOption.changeColor = changeColor;
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

    /// <summary>
    /// ���� �������� �ǵ�����
    /// </summary>
    public void Revart()
    {
        // ���� �������� ��ü �ǵ�����
        Material avatarMat = avatar.transform.GetComponent<Image>().material;
        foreach(var part in customizingOptionDataList)
        {
            SetColorByPart(avatarMat, part.materialName, part.originColor);
        }
    }

    /// <summary>
    /// ���� �õ�
    /// </summary>
    public void TryPurchase()
    {
        int totalPirce = 0;

        foreach(var choice in customizingOptionDataList)
        {
            if(choice.selectSlotIdx != -1)
            {
                // ������ �׸��� �ִٸ� price �߰�
                totalPirce += choice.price;
            }
        }

        // ���� ����� �����ϴٸ� NoticeUI ����
        if(totalPirce < GameManager.Instance.Player.CurrentCoin)
        {
            Debug.Log("���� �����մϴ�!");
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
