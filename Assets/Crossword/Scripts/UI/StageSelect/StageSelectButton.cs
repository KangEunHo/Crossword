﻿using System.Collections;
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
            CompleteData completeData = SaveMgr.Instance.GetCompleteData(stageIndex);
            if (completeData == null)
            {
                // 투두. 로드 프로그래스.
                // SaveMgr.Instance.
                star.SetActive(false);
            }
            else
            {
                progressCircle.fillAmount = 1f;
                star.SetActive(completeData.perfectClear);
            }

            stageText.text = (stageIndex + 1).ToString();
            this.stageIndex = stageIndex;
        }
    }
}