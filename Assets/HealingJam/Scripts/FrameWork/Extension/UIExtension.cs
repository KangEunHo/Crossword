using UnityEngine;
using UnityEngine.UI;

public static class UIExtension
{
    public static void SetAlpha(this Graphic graphic, float a)
    {
        Color color = graphic.color;
        color.a = a;
        graphic.color = color;
    }

    public static void SetAlpha(ref this Color color, float a)
    {
        color.a = a;
    }

    public static Color NewAlpha(this Color color, float a)
    {
        color.a = a;
        return color;
    }
}
