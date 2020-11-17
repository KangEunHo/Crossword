using UnityEngine;
using UnityEngine.UI;

namespace HealingJam
{
    public class CanvasScalerSetter : MonoBehaviour
    {
        private void Awake()
        {
            float match = UnityEngine.Screen.height / 16f * 9f >= UnityEngine.Screen.width ? 0f : 1f;
            var canvasScalers = GetComponentsInChildren<CanvasScaler>();

            for (int i = 0; i < canvasScalers.Length; ++i)
            {
                canvasScalers[i].matchWidthOrHeight = match;
            }
        }
    }
}