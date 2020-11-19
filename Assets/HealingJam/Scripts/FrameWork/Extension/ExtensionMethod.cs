using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class ExtensionMethod
{
    //public static Rect ActualRect(this RectTransform trans)
    //{
    //    return RectTransformUtility.PixelAdjustRect(trans, trans.GetComponentInParent<Canvas>());
    //}

    public static Vector2 ClampPoint(this Rect rect, Vector2 point)
    {
        point.x = Mathf.Clamp(point.x, rect.xMin, rect.xMax);
        point.y = Mathf.Clamp(point.y, rect.yMin, rect.yMax);
        return point;
    }

    //public static void SetLeft(this RectTransform rt, float left)
    //{
    //    rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    //}

    //public static void SetRight(this RectTransform rt, float right)
    //{
    //    rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    //}

    //public static void SetTop(this RectTransform rt, float top)
    //{
    //    rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    //}

    //public static void SetBottom(this RectTransform rt, float bottom)
    //{
    //    rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    //}

    //public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
    //{
    //    Vector3 deltaPosition = rectTransform.pivot - pivot;    // get change in pivot
    //    deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
    //    deltaPosition.Scale(rectTransform.localScale);          // apply scaling
    //    deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation

    //    rectTransform.pivot = pivot;                            // change the pivot
    //    rectTransform.localPosition -= deltaPosition;           // reverse the position change
    //}

    /// <summary>
    /// Gets the orthographics bounds for the camera.
    /// </summary>
    /// <returns>The bounds.</returns>
    /// <param name="camera">Camera.</param>
    public static Bounds OrthographicBounds(this Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            if (k != n)
            {
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static T RandomValue<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}