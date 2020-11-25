using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HealingJam.Crossword
{
    public class DarkModeColor : DarkModeMonoBehaviour
    {
        public Color darkModeColor = Color.white;
        public Color lightModeColor = Color.black;
        public Graphic graphic = null;

        public override void DarkModeChanged(bool darkMode)
        {
            if (graphic == null)
                graphic = GetComponent<Graphic>();

            graphic.color = darkMode ? darkModeColor : lightModeColor;

            this.darkMode = darkMode;
        }
    }
}