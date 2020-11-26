using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword.UI
{
    public class StatisticsController : MonoBehaviour
    {
        private const float BRANCH_WIDTH = 16f;
        private const float BRANCH_MAX_HEIGHT = 384f;
        private const float BRANCH_ANIMATION_SPEED = 0.5f;

        [SerializeField] private Image bottomGauge = null;
        [SerializeField] private RectTransform[] branches = null;

        private Sequence animationSequence = null;

        public void SetUp(int min, int max)
        {
            List<int> wrongCountsByType = new List<int>();
            while (wrongCountsByType.Count < (int)WordData.WordType.Max)
            {
                wrongCountsByType.Add(0);
            }

            for (int i = min; i < max; ++i)
            {
                LevelData levelData = SaveMgr.Instance.GetLevelData(min);

                foreach(var v in levelData.wrongCountsByType.Keys)
                {
                    wrongCountsByType[(int)v] += levelData.wrongCountsByType[v];
                }
            }

#if UNITY_EDITOR
            //테스트.
            wrongCountsByType[9] = 2;
            wrongCountsByType[1] = 1;

            wrongCountsByType[2] = 5;
            wrongCountsByType[4] = 7;
            wrongCountsByType[7] = 8;
            wrongCountsByType[6] = 10;

#endif

            int answerCount = (max - min) * 2;
            PlayAnimation(answerCount, wrongCountsByType);
        }

        private void PlayAnimation(int answerCount, List<int> wrongCountsByType)
        {
            bottomGauge.fillAmount = 0f;
            foreach(var branch in branches)
            {
                branch.sizeDelta = new Vector2(BRANCH_WIDTH, 0f);
            }
            
            if (animationSequence != null)
            {
                animationSequence.Kill(false);
            }

            animationSequence = DOTween.Sequence()
                .Append(bottomGauge.DOFillAmount(1f, 0.2f));
            
            for (int i = 1; i < wrongCountsByType.Count; ++i)
            {
                float percentageOfCorrectAnswers = 1 - (wrongCountsByType[i] / (float)answerCount);

                float height = Mathf.Max(20f, BRANCH_MAX_HEIGHT * percentageOfCorrectAnswers);
                if (i == 1)
                    animationSequence.Append(branches[i -1].DOSizeDelta(new Vector2(BRANCH_WIDTH, height),
                        BRANCH_ANIMATION_SPEED * percentageOfCorrectAnswers));
                else
                    animationSequence.Join(branches[i -1].DOSizeDelta(new Vector2(BRANCH_WIDTH, height),
                        BRANCH_ANIMATION_SPEED * percentageOfCorrectAnswers));
            }


        }
    }
}