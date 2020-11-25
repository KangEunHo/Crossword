using UnityEngine;
using System.Collections;

namespace HealingJam.Crossword
{
    public abstract class DarkModeMonoBehaviour : MonoBehaviour, IDarkModeChangeable
    {
        protected bool? darkMode;

        protected void OnEnable()
        {
            if (darkMode == null || darkMode != DarkMode.UseDarkMode)
            {
                DarkModeChanged(DarkMode.UseDarkMode);
            }
            DarkMode.DrakModeChangeAction += DarkModeChanged;
        }

        protected void OnDisable()
        {
            DarkMode.DrakModeChangeAction -= DarkModeChanged;
        }

        public abstract void DarkModeChanged(bool darkMode);
    }
}