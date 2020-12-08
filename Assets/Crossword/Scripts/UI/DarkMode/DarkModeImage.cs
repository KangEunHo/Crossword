using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HealingJam.Crossword
{
    [RequireComponent(typeof(Image))]
    public class DarkModeImage : DarkModeMonoBehaviour
    {
        public Sprite lightModeSprite = null;
        public Sprite darkModeSprite = null;

        public Image image = null;

        public override void DarkModeChanged(bool darkMode)
        {
            this.darkMode = darkMode;
            if (image == null)
                image = GetComponent<Image>();

            image.sprite = darkMode ? darkModeSprite : lightModeSprite;

        }
    }
}