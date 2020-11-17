using UnityEngine;

namespace HealingJam
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeNotch : MonoBehaviour
    {
#if UNITY_IOS
    private void Start()
    {
        if (IsIOSNotchDesign())
        {
            float delta = (Screen.height - Screen.safeArea.height) / (float)Screen.height;
            float padding = delta * Screen.height;

            RectTransform rt = GetComponent<RectTransform>();
            rt.offsetMax = new Vector2(rt.offsetMax.x, -padding * 0.5f);
        }
    }
#endif

        public static bool IsIOSNotchDesign()
        {
            return Screen.height > Screen.safeArea.height;
        }
    }
}