using UnityEngine;
using System.Collections;

namespace HealingJam.Crossword
{
    public abstract class DarkModeMonoBehaviour : MonoBehaviour, IDarkModeChangeable
    {
        public bool startOneShot = true;

        protected void Start()
        {
            if (startOneShot)
                DarkModeChanged(DarkMode.UseDarkMode);
        }

        protected void OnEnable()
        {
            DarkMode.DrakModeChangeAction += DarkModeChanged;
        }

        protected void OnDisable()
        {
            DarkMode.DrakModeChangeAction -= DarkModeChanged;
        }

        public abstract void DarkModeChanged(bool darkMode);
    }
}