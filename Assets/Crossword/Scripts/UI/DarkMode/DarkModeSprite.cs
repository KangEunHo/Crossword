using UnityEngine;
using System;

namespace HealingJam.Crossword
{
    [Serializable]
    public class DarkModeSprite
    {
        public Sprite ActiveModeSprite { get
            {
                if (DarkMode.UseDarkMode)
                    return darkModeSprite;
                else return lightModeSprite;
            } }
        public Sprite darkModeSprite = null;
        public Sprite lightModeSprite = null;
    }
}