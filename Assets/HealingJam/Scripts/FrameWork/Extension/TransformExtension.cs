using UnityEngine;
using System.Collections.Generic;

public static class TransformExtension
{
    public static void SetPositionX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public static void SetPositionY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public static void SetPositionZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    public static void SetLocalPositionX(this Transform transform, float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    public static void SetLocalPositionY(this Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }

    public static void SetLocalPositionZ(this Transform transform, float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }

    public static void SetLocalScaleX(this Transform transform, float x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public static void SetLocalScaleY(this Transform transform, float y)
    {
        transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
    }

    public static void SetLocalScaleZ(this Transform transform, float z)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
    }

    public static void SetAnchoredPositionX(this RectTransform transform, float x)
    {
        transform.anchoredPosition = new Vector2(x, transform.anchoredPosition.y);
    }

    public static void SetAnchoredPositionY(this RectTransform transform, float y)
    {
        transform.anchoredPosition = new Vector2(transform.anchoredPosition.x, y);
    }

    //Breadth-first search
    public static Transform FindChildBFS(this Transform parent, string name)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(parent);
        while (queue.Count > 0)
        {
            var t = queue.Dequeue();
            if (t.name == name)
                return t;
            int childCount = t.childCount;

            for (int i = 0; i < childCount; ++i)
            {
                queue.Enqueue(t.GetChild(i));
            }
        }
        return null;
    }


    //Depth-first search
    public static Transform FindChildDFS(this Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;
            var result = child.FindChildDFS(name);
            if (result != null)
                return result;
        }
        return null;
    }


    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
    {
        Vector3 deltaPosition = rectTransform.pivot - pivot;    // get change in pivot
        deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
        deltaPosition.Scale(rectTransform.localScale);          // apply scaling
        deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation

        rectTransform.pivot = pivot;                            // change the pivot
        rectTransform.localPosition -= deltaPosition;           // reverse the position change
    }
}
