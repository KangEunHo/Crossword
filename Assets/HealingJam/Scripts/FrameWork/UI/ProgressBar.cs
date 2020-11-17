using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HealingJam
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image frontGauge = null;

        public void SetProgress(float value)
        {
            frontGauge.fillAmount = value;
        }
    }
}