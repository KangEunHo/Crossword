using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HealingJam.Crossword.Save;
using System;

namespace HealingJam.Crossword.UI
{
    [RequireComponent(typeof(Button))]
    public class StageSelectButton : MonoBehaviour
    {
        [SerializeField] private Text stageText = null;

        [SerializeField] private Image progressCircle = null;
        [SerializeField] private GameObject star = null;

        public Action<int> onClickAction = null;
        private int stageIndex;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => { onClickAction?.Invoke(stageIndex); });
        }

        public void SetUp(int stageIndex)
        {
            bool completeData = SaveMgr.Instance.GetCompleteData(stageIndex);
            if (completeData)
            {
                progressCircle.fillAmount = 1f;
            }
            else
            {
                if (SaveMgr.Instance.TryGetProgressData(stageIndex, out ProgressData progressData))
                {
                    progressCircle.fillAmount = progressData.machedWords.Count / (float)CrosswordMapManager.Instance.GetCrosswordMap(stageIndex).wordDatas.Count;
                }
                else
                {
                    progressCircle.fillAmount = 0f;
                }
            }

            star.SetActive(completeData);
            stageText.text = (stageIndex + 1).ToString();
            this.stageIndex = stageIndex;
        }
    }
}