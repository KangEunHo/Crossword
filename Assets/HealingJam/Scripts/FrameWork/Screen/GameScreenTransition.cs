using UnityEngine;
using DG.Tweening;

namespace HealingJam.GameScreens
{
    public class GameScreenTransition : MonoBehaviour
    {
        public enum XMoveDir : int
        {
            Left = -1, None = 0, Right = 1
        }

        public enum YMoveDir : int
        {
            Bottom = -1, None = 0, Top = 1
        }

        [Tooltip("millisecond duration")]
        [SerializeField] private float animationDuration = 350f;
        public bool IsAnimating { get; private set; } = false;


        public void SwitchScreen(GameScreen.ScreenID screenID, XMoveDir xMoveDir, YMoveDir yMoveDir, object[] exitParams, params object[] enterParams)
        {
            if (IsAnimating)
                return;

            IsAnimating = true;

            GameScreen currentScreen = ScreenMgr.Instance.GetCurrentScreen();
            RectTransform currentScreenRT = currentScreen == null ? null : currentScreen.transform as RectTransform;
            ScreenMgr.Instance.Enter(screenID, enterParams);
            RectTransform newScreenRT = ScreenMgr.Instance.GetCurrentScreen().transform as RectTransform;

            Vector2 screenSize = new Vector2(UIUtilities.GetScreenWidth(newScreenRT), UIUtilities.GetScreenHeight(newScreenRT));

            if (currentScreenRT != null)
            {
                currentScreenRT.SetAnchoredPositionX(0f);
                currentScreenRT.SetAnchoredPositionY(0f);

                float toX = -(int)xMoveDir * screenSize.x;
                float toY = -(int)yMoveDir * screenSize.y;

                currentScreenRT.DOAnchorPos(new Vector2(toX, toY), animationDuration)
                    .OnComplete(() => { ScreenMgr.Instance.Exit(currentScreen.ID, exitParams); });
            }

            newScreenRT.anchoredPosition = new Vector2((int)xMoveDir * screenSize.x, (int)yMoveDir * screenSize.y);
            newScreenRT.DOAnchorPos(Vector2.zero, animationDuration)
            .OnComplete(() => { IsAnimating = false; });
        }
    }
}