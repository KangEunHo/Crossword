using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword.UI
{
    public class ResultGaugeController : MonoBehaviour
    {
        private const float GAUGE_WIDTH = 24f;
        private const float GAUGE_MAX_HEIGHT = 232f;
        private const float GAUGE_ANIMATION_SPEED = 1f;

        [SerializeField] private RectTransform[] guages = null;

        private Sequence animationSequence = null;


        public void PlayAnimation(List<RightAnswerCountData> rightAnswerCountDatas)
        {
            foreach (var guage in guages)
            {
                guage.sizeDelta = new Vector2(GAUGE_WIDTH, 0f);
            }

            if (animationSequence != null)
            {
                animationSequence.Kill(false);
            }

            animationSequence = DOTween.Sequence();

            for (int i = 1; i < rightAnswerCountDatas.Count; ++i)
            {
                RightAnswerCountData rightAnswerCountData = rightAnswerCountDatas[i];
                float percentageOfCorrectAnswers = rightAnswerCountData.rightAnswerCount / (float)rightAnswerCountData.answerCount;

                float height = GAUGE_MAX_HEIGHT * percentageOfCorrectAnswers;
                if (i == 1)
                    animationSequence.Append(guages[i - 1].DOSizeDelta(new Vector2(GAUGE_WIDTH, height),
                        GAUGE_ANIMATION_SPEED * percentageOfCorrectAnswers));
                else
                    animationSequence.Join(guages[i - 1].DOSizeDelta(new Vector2(GAUGE_WIDTH, height),
                        GAUGE_ANIMATION_SPEED * percentageOfCorrectAnswers));
            }
        }
    }
}