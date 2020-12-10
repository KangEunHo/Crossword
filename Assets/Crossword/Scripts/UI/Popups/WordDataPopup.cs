using UnityEngine;
using System.Collections.Generic;
using HealingJam.Popups;
using UnityEngine.UI;
using HealingJam.Crossword.UI;

namespace HealingJam.Crossword
{
    public class WordDataPopup : Popup
    {
        private const int ZOOM_FONT_SIZE = 36;
        private const int BASIC_FONT_SIZE = 24;

        [SerializeField] private Button nextButton = null;
        [SerializeField] private Button prevButton = null;
        [SerializeField] private Text wordText = null;
        [SerializeField] private Text meaningText = null;

        [SerializeField] private ImageSwap resultImageSwap = null;
        [SerializeField] private ImageSwap zoomImageSwap = null;

        private int curPage = 0;
        private bool isZoomed = false;
        public int MaxPage => answerItemDatas.Count;
        private List<AnswerItem.AnswerItemData> answerItemDatas = null;


        public override void Enter(params object[] args)
        {
            base.Enter(args);

            answerItemDatas = args[1] as List<AnswerItem.AnswerItemData>;

            int page = (int)args[0];

            if (answerItemDatas.Count > 0)
                SetPage(page);
        }

        private void SetPage(int page)
        {
            curPage = page;
            WordData wordData = answerItemDatas[page].wordData;
            resultImageSwap.SetSprite(answerItemDatas[page].correctAnswer);

            wordText.text = wordData.word;
            string typeStr = "[" + WordMeaningController.WordTypeToString(wordData.wordType) + "]";
            meaningText.text = typeStr + "\n" + wordData.info;

            ContentSizeUpdated();

            bool nextButtonInteractable = curPage + 1 < MaxPage;
            bool prevButtonInteractable = curPage - 1 >= 0;
            nextButton.interactable = nextButtonInteractable;
            nextButton.image.SetAlpha(nextButtonInteractable ? 1f : 0.5f);
            prevButton.interactable = prevButtonInteractable;
            prevButton.image.SetAlpha(prevButtonInteractable ? 1f : 0.5f);

        }

        public void OnNextButtonClick()
        {
            SetPage(curPage + 1);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnPrevButtonClick()
        {
            SetPage(curPage - 1);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnZoomButtonClick()
        {
            isZoomed = !isZoomed;

            if (isZoomed)
                OnZoomText();
            else
                OffZoomText();

            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        private void ContentSizeUpdated()
        {
            meaningText.rectTransform.sizeDelta = meaningText.rectTransform.sizeDelta.NewY(meaningText.preferredHeight);
        }

        private void OnZoomText()
        {
            meaningText.fontSize = ZOOM_FONT_SIZE;
            ContentSizeUpdated();
            zoomImageSwap.SetSprite(false);
        }

        private void OffZoomText()
        {
            meaningText.fontSize = BASIC_FONT_SIZE;
            ContentSizeUpdated();
            zoomImageSwap.SetSprite(true);
        }
    }
}