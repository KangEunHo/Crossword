using UnityEngine;
using System.Collections;

namespace HealingJam
{
    [RequireComponent(typeof(Camera))]
    public class CameraOrthographicSetter : MonoBehaviour
    {
        private void Awake()
        {
            float orthographicSize = UnityEngine.Screen.height / 16f * 9f >= UnityEngine.Screen.width ? 6.24f : 5.12f;
            GetComponent<Camera>().orthographicSize = orthographicSize;
        }
    }
}