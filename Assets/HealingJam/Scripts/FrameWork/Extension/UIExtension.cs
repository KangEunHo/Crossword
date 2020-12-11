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

    // 스크롤 렉트의 콘텐츠의 중앙 위치를 자식위치로 맞춥니다.
    public static void SnapTo(this ScrollRect scroller, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();

        var contentPos = (Vector2)scroller.transform.InverseTransformPoint(scroller.content.position);
        var childPos = (Vector2)scroller.transform.InverseTransformPoint(child.position);
        var endPos = contentPos - childPos;

        Vector2 pivotDelta = (Vector2.one * 0.5f) - scroller.content.pivot;
        endPos += pivotDelta * scroller.viewport.rect.size;

        // If no horizontal scroll, then don't change contentPos.x
        if (!scroller.horizontal)
        {
            endPos.x = scroller.content.anchoredPosition.x;
        }
        else
        {
            float x1 = (scroller.content.pivot.x - 1) * scroller.content.rect.size.x;
            float x2 = (scroller.content.pivot.x) * scroller.content.rect.size.x;
            endPos.x = Mathf.Clamp(endPos.x, Mathf.Min(x1, x2), Mathf.Max(x1, x2));
        }
        // If no vertical scroll, then don't change contentPos.y
        if (!scroller.vertical)
        {
            endPos.y = scroller.content.anchoredPosition.y;
        }
        else
        {
            float y1 = (scroller.content.pivot.y -1) * scroller.content.rect.size.y;
            float y2 = (scroller.content.pivot.y) * scroller.content.rect.size.y;
            endPos.y = Mathf.Clamp(endPos.y, Mathf.Min(y1, y2), Mathf.Max(y1, y2));
        }

        scroller.content.anchoredPosition = endPos;
    }
}
