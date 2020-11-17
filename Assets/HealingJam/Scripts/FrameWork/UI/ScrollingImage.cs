using UnityEngine;

namespace HealingJam
{
    /// <summary>
    /// 두개의 이미지를 이용해 무한스크롤 배경을 만듭니다.
    /// 두 이미지의 anchoredPosition을 anchors, pivot의 x 를 0으로 설정해야합니다.
    /// </summary>
    public class ScrollingImage : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private RectTransform[] transfromsToScroll = null;
        [SerializeField] private bool playOnAwake = false;
        [SerializeField] private float moveSpeed = 10f;

        #endregion

        #region Member Variables

        private float width;
        private bool isPlaying = false;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (playOnAwake)
                Play();
        }

        private void Update()
        {
            if (isPlaying)
            {
                for (int i = 0; i < 2; ++i)
                {
                    float xPos = transfromsToScroll[i].anchoredPosition.x;
                    xPos -= moveSpeed * Time.deltaTime;

                    if (xPos <= -width)
                    {
                        xPos += width * 2f;
                    }
                    transfromsToScroll[i].anchoredPosition = new Vector2(xPos, transfromsToScroll[1].anchoredPosition.y);
                }
            }
        }
        #endregion

        #region Public Methods

        public void Play()
        {
            isPlaying = true;
            width = transfromsToScroll[0].rect.width * transfromsToScroll[0].localScale.x;
            transfromsToScroll[0].anchoredPosition = Vector2.zero;
            transfromsToScroll[1].anchoredPosition = new Vector2(width, transfromsToScroll[1].anchoredPosition.y);
        }

        #endregion
    }
}