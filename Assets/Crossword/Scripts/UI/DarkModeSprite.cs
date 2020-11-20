using UnityEngine;
using System;

namespace HealingJam.Crossword
{
    [Serializable]
    public class DarkModeSprite : IDarkModeChangeable
    {
        [NonSerialized]
        public Sprite activeModeSprite = null;
        public Sprite darkModeSprite = null;
        public Sprite whiteModeSprite = null;

        public DarkModeSprite()
        {
            DarkModeChanged(DarkMode.UseDarkMode);
        }

        public void DarkModeChanged(bool darkMode)
        {
            activeModeSprite = darkMode ? darkModeSprite : whiteModeSprite;
        }
    }
}