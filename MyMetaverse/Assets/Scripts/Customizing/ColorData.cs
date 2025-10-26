using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EColor
{
    Red, Blue, Green,
    Pink, Orange, Yellow,
    Black, White, Purple,
    Brown, Cyan, Lime,

    Gray, DarkGray,
}

public class ColorData
{
    private static List<Color> colors = new List<Color>()
    {
        new Color(1f, 0f, 0f),
        new Color(0.1f, 0.1f, 1f),
        new Color(0f, 0.6f, 0f),
        new Color(1f, 0.3f, 0.9f),
        new Color(1f, 0.4f, 0f),
        new Color(1f, 0.9f, 0.1f),
        new Color(0.2f, 0.2f, 0.2f),
        new Color(0.9f, 1f, 1f),
        new Color(0.6f, 0f, 0.6f),
        new Color(0.7f, 0.2f, 0f),
        new Color(0f, 1f, 1f),
        new Color(0.1f, 0f, 0.1f),
        new Color(0.9137f, 0.9019f, 0.9019f),
        new Color(0.6627f, 0.6627f, 0.6627f),
    };

    public static int GetColorCount() {  return colors.Count; }
    public static Color GetColor(EColor playerColor) { return colors[(int)playerColor]; }
    public static Color GetColor(int idx) { return colors[idx]; }

    public static Color Red { get { return colors[(int)EColor.Red]; } }
    public static Color Blue { get { return colors[(int)EColor.Blue]; } }
    public static Color Green { get { return colors[(int)EColor.Green]; } }
    public static Color Pink { get { return colors[(int)EColor.Pink]; } }
    public static Color Orange { get { return colors[(int)EColor.Orange]; } }
    public static Color Yellow { get { return colors[(int)EColor.Yellow]; } }
    public static Color Black { get { return colors[(int)EColor.Black]; } }
    public static Color White { get { return colors[(int)EColor.White]; } }
    public static Color Purple { get { return colors[(int)EColor.Purple]; } }
    public static Color Brown { get { return colors[(int)EColor.Brown]; } }
    public static Color Cyan { get { return colors[(int)EColor.Cyan]; } }
    public static Color Lime { get { return colors[(int)EColor.Lime]; } }
    public static Color Origin { get { return colors[(int)EColor.Gray]; } }
    public static Color Highlight { get { return colors[(int)EColor.DarkGray]; } }
}
