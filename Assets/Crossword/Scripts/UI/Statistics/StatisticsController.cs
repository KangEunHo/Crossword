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
        private const float BRANCH_ANIMATION_SPEED = 0.5f;

        [SerializeField] private RectTransform treeRT = null;
        [SerializeField] private Image bottomGauge = null;
        [SerializeField] private RectTransform[] branches = null;
        [SerializeField] private Text topText = null;

        private Sequence animationSequence = null;

        public void PlayAnimation(int badgeIndex)
        {
            List<RightAnswerCountData> rightAnswerCountData = new List<RightAnswerCountData>();

            for (int i = 0; i< (int)WordData.WordType.Max; ++i)
            {
                rightAnswerCountData.Add(new RightAnswerCountData());
            }

            for (int i = 0; i < CrosswordMapManager.BADGE_IN_LEVEL_COUNT; ++i)
            {
                int levelIndex = badgeIndex * CrosswordMapManager.BADGE_IN_LEVEL_COUNT + i;
                LevelData levelData = SaveMgr.Instance.GetLevelData(levelIndex);
                if (levelData.completed == false || levelData.rightAnswerCountDatas.Count == 0)
                {
                    for (int j = 1; j < (int)WordData.WordType.Max; ++j)
                    {
                        rightAnswerCountData[j].answerCount += 2;
                    }
                    continue;
                }

                for (int j = 1; j < (int)WordData.WordType.Max; ++j)
                {
                    rightAnswerCountData[j].answerCount += levelData.rightAnswerCountDatas[j].answerCount;
                    rightAnswerCountData[j].rightAnswerCount += levelData.rightAnswerCountDatas[j].rightAnswerCount;
                }
            }

            SetTopText((badgeIndex * CrosswordMapManager.BADGE_IN_LEVEL_COUNT) + 1, badgeIndex * CrosswordMapManager.BADGE_IN_LEVEL_COUNT + CrosswordMapManager.BADGE_IN_LEVEL_COUNT);
            //for (int j = 1; j < (int)WordData.WordType.Max; ++j)
            //{
            //    rightAnswerCountData[j].answerCount += 2 * CrosswordMapManager.BADGE_IN_LEVEL_COUNT;
            //}

            PlayAnimation(rightAnswerCountData);
        }

        public void PlayAnimationAll()
        {
            List<RightAnswerCountData> rightAnswerCountData = new List<RightAnswerCountData>();

            for (int i = 0; i < (int)WordData.WordType.Max; ++i)
            {
                rightAnswerCountData.Add(new RightAnswerCountData());
            }

            int unlockLevel = SaveMgr.Instance.GetUnlockLevel();
            for (int i = 0; i< unlockLevel; ++i)
            {
                int levelIndex = i;
                LevelData levelData = SaveMgr.Instance.GetLevelData(levelIndex);
                if (levelData.completed == false || levelData.rightAnswerCountDatas.Count == 0)
                    continue;

                for (int j = 1; j < (int)WordData.WordType.Max; ++j)
                {
                    rightAnswerCountData[j].answerCount += levelData.rightAnswerCountDatas[j].answerCount;
                    rightAnswerCountData[j].rightAnswerCount += levelData.rightAnswerCountDatas[j].rightAnswerCount;
                }
            }

            SetTopText(1, Mathf.Max(1, unlockLevel));

            //for (int j = 1; j < (int)WordData.WordType.Max; ++j)
            //{
            //    rightAnswerCountData[j].answerCount += 2 * CrosswordMapManager.BADGE_IN_LEVEL_COUNT;
            //}

            PlayAnimation(rightAnswerCountData);
        }

        private void PlayAnimation(List<RightAnswerCountData> rightAnswerCountData)
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

            float maxHeight = treeRT.rect.height - 178f;
            for (int i = 1; i < (int)WordData.WordType.Max; ++i)
            {
                float percentageOfCorrectAnswers = rightAnswerCountData[i].rightAnswerCount / (float)rightAnswerCountData[i].answerCount;

                float height = Mathf.Max(20f, maxHeight * percentageOfCorrectAnswers);
                if (i == 1)
                    animationSequence.Append(branches[i -1].DOSizeDelta(new Vector2(BRANCH_WIDTH, height),
                        BRANCH_ANIMATION_SPEED * percentageOfCorrectAnswers));
                else
                    animationSequence.Join(branches[i -1].DOSizeDelta(new Vector2(BRANCH_WIDTH, height),
                        BRANCH_ANIMATION_SPEED * percentageOfCorrectAnswers));

            }
        }

        private void SetTopText(int start, int end)
        {
            topText.text = string.Format("{0}~{1} 레벨 테스트의 통계 자료입니다.", start, end);
        }
    }
}