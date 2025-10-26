using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizingSlot : MonoBehaviour
{
    [SerializeField] public int Idx { get; set; }
    [SerializeField] public Sprite Img { get; set; }

    public void Init(int idx, Sprite img)
    {
        Img = img;
        Idx = idx;
    }
}
