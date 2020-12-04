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
        private int packIndex;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => { onClickAction?.Invoke(packIndex); });
        }

        public void SetUp(int packIndex)
        {
            bool completeData = SaveMgr.Instance.GetCompleteData(packIndex);
            if (completeData)
            {
                progressCircle.fillAmount = 1f;
            }
            else
            {
                if (SaveMgr.Instance.TryGetProgressData(packIndex, out ProgressData progressData))
                {
                    progressCircle.fillAmount = progressData.machedWords.Count / (float)CrosswordMapManager.Instance.GetCrosswordMap(packIndex).wordDatas.Count;
                }
                else
                {
                    progressCircle.fillAmount = 0f;
                }
            }

            star.SetActive(completeData);
            stageText.text = (packIndex + 1).ToString();
            this.packIndex = packIndex;
        }
    }
}