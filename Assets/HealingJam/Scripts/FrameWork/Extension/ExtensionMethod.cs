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