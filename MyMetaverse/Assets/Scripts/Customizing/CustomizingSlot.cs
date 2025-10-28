using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingSlot : MonoBehaviour
{
    [SerializeField] public int Idx { get; set; }
    [SerializeField] public Sprite Img { get; set; }

    [Header("Slot Object")]
    [SerializeField] private Button slotBtn;
    [SerializeField] private Image slotBG;
    [SerializeField] private Image slotBan;
    [SerializeField] private Image slotIcon;
    [SerializeField] private Image slotHighlight;

    private UnityEngine.Color whiteColor;
    private UnityEngine.Color darkGrayColor;

    private void Start()
    {
        whiteColor = ColorData.GetColor(EColor.White);
        darkGrayColor = ColorData.GetColor(EColor.DarkGray);
    }

    public void InitSlot(int idx, Sprite iconSprite, UnityEngine.Color iconColor)
    {
        this.Idx = idx;

        Material iconMat = new Material(slotIcon.material);
        slotIcon.material = iconMat;

        SetColorSlotIcon(iconColor);
        SetSpriteSlotIcon(iconSprite);

        ResetSlot();
    }

    public void ResetSlot()
    {
        slotBG.color = whiteColor;
        slotHighlight.gameObject.SetActive(false); 
        slotBan.gameObject.SetActive(false);
    }

    public void SetColorSlotIcon(UnityEngine.Color slotColor)
    {
        Material iconMat = slotIcon.material;
        iconMat.color = slotColor;
    }

    public void SetSpriteSlotIcon(Sprite sprite)
    {
        slotIcon.sprite = sprite;
    }

    public void SetColorSlotBG(UnityEngine.Color bgColor)
    {
        slotBG.color = bgColor;
    }

    public void BanSlot()
    {
        // ΩΩ∑‘ πÍ
        slotBtn.interactable = false;
        SetColorSlotBG(darkGrayColor);
        slotBan.gameObject.SetActive(true);
    }

    public void HighlightSlot()
    {
        // ΩΩ∑‘ «œ¿Ã∂Û¿Ã∆Æ
        SetColorSlotBG(darkGrayColor);
        slotHighlight.gameObject.SetActive(true);
    }
}
