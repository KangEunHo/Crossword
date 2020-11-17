using UnityEngine;
using UnityEngine.UI;

namespace HealingJam
{
    public static class UIUtilities
    {
        public static int BasicScreenWidth = 576;
        public static int BasicScreenHeight = 1024;
        public static float MatchWidthOrHeight { get { return UnityEngine.Screen.height / 16f * 9f >= UnityEngine.Screen.width ? 0f : 1f; } }

        public static float GetScreenWidth(RectTransform rectTransform)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();

            return GetScreenWidth(canvas);
        }

        public static float GetScreenHeight(RectTransform rectTransform)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();

            return GetScreenHeight(canvas);
        }

        public static float GetScreenWidth(Canvas canvas)
        {
            return UnityEngine.Screen.width * (1f / canvas.scaleFactor);
        }

        public static float GetScreenHeight(Canvas canvas)
        {
            return UnityEngine.Screen.height * (1f / canvas.scaleFactor);
        }

        public static void SetResolutionScale(Transform transform)
        {
            float baseRatio = (float)BasicScreenWidth / BasicScreenHeight * Screen.height;
            float percentScale = Screen.width / baseRatio;
            if (percentScale < 1)
                transform.localScale = new Vector3(transform.localScale.x * percentScale, transform.localScale.y * percentScale, 1);
        }

        public static bool IsContainsInRect(Vector2 screenPos, RectTransform rectTransform, out Vector2 localPos)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, null, out localPos) == false)
                return false;

            localPos += rectTransform.rect.size * 0.5f;
            if (localPos.x < 0 || localPos.y < 0 || localPos.x > rectTransform.rect.width || localPos.y > rectTransform.rect.height)
                return false;

            return true;
        }

        public static void ClampScreenPointInRect(Vector2 screenPos, RectTransform rectTransform, out Vector2 localPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, null, out localPos);

            localPos += rectTransform.rect.size * 0.5f;

            localPos.x = Mathf.Clamp(localPos.x, rectTransform.rect.xMin, rectTransform.rect.xMax);
            localPos.y = Mathf.Clamp(localPos.y, rectTransform.rect.yMin, rectTransform.rect.yMax);
        }

        public static Rect GetScreenCoordinates(RectTransform uiElement)
        {
            var worldCorners = new Vector3[4];
            uiElement.GetWorldCorners(worldCorners);
            var result = new Rect(
                          worldCorners[0].x,
                          worldCorners[0].y,
                          worldCorners[2].x - worldCorners[0].x,
                          worldCorners[2].y - worldCorners[0].y);
            return result;
        }

        public static string LetterWrapText(Text text, string str)
        {
            float rectWidth = text.rectTransform.rect.width;

            char[] s = str.ToCharArray();
            int len = s.Length;
            string newStr = "";
            string lineStr = "";

            for (int textPosition = 0; textPosition < len; textPosition++)
            {
                lineStr += s[textPosition];
                text.text = lineStr;

                if (s[textPosition] == '\n')
                {
                    newStr += lineStr;
                    lineStr = "";
                }
                else if (text.preferredWidth >= rectWidth)
                {
                    lineStr = lineStr.Remove(lineStr.Length - 1);
                    newStr += lineStr + '\n';
                    lineStr = "";
                    textPosition--;
                }
            }

            if (lineStr.Length > 0)
            {
                newStr += lineStr;
            }

            text.text = "";

            return newStr;
        }
    }
}