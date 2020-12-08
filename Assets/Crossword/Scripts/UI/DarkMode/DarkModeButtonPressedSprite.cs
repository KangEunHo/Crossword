using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HealingJam.Crossword
{
    [RequireComponent(typeof(Button))]
    public class DarkModeButtonPressedSprite : DarkModeMonoBehaviour
    {
        public Sprite lightModePressedSprite = null;
        public Sprite darkModePressedSprite = null;

        public Button button = null;

        public override void DarkModeChanged(bool darkMode)
        {
            this.darkMode = darkMode;

            if (button == null)
                button = GetComponent<Button>();

            SpriteState spriteState = button.spriteState;
            spriteState.pressedSprite = darkMode ? darkModePressedSprite : lightModePressedSprite;
            button.spriteState = spriteState;
        }
    }
}