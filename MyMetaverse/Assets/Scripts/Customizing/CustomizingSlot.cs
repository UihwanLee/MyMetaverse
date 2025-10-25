using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizingSlot : MonoBehaviour
{
    public int Idx { get; set; }
    public Sprite Img { get; set; }

    public void Init(int idx, Sprite img)
    {
        Img = img;
        Idx = idx;
    }
}
