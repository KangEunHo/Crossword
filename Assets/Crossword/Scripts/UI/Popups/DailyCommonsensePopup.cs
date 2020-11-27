using UnityEngine;
using System.Collections.Generic;
using HealingJam.Popups;
using System;
using UnityEngine.UI;

namespace HealingJam.Crossword
{
    public struct DailyCommonsense
    {
        public string date;
        public string word;
        public string meaning;
    }

    public class DailyCommonsensePopup : Popup
    {
        private const int ZOOM_FONT_SIZE = 36;
        private const int BASIC_FONT_SIZE = 24;

        public static DateTime KOREA_TIME { get { return DateTime.UtcNow.AddHours(9); } }

        public static DateTime LAST_VIEW_TIME
        {
            get {
                string lastVeiwTime = PlayerPrefs.GetString("last_view_time", "19700101");

                if (DateTime.TryParseExact(lastVeiwTime, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                else
                    return new DateTime(1970, 1, 1);
            }
            set
            {
                PlayerPrefs.SetString("last_view_time", value.ToString("yyyyMMdd"));
            }
        }

        public static bool GET_LAST_DAILY_REAWRD
        {
            get{ return PlayerPrefs.GetInt("get_last_daily_reward", 0) == 1 ? true : false;}
            set { PlayerPrefs.SetInt("get_last_daily_reward", value ? 1 : 0); }
        }

        [SerializeField] private Button nextButton = null;
        [SerializeField] private Button prevButton = null;
        [SerializeField] private Text wordText = null;
        [SerializeField] private Text meaningText = null;
        [SerializeField] private Text dateText = null;
        [SerializeField] private ImageSwap zoomImageSwap = null;

        private int page = 0;
        private bool isZoomed = false;
        public int MaxPage => dailyCommonsenses.Count;

        private List<DailyCommonsense> dailyCommonsenses = null;


        public override void Enter(params object[] args)
        {
            base.Enter(args);

            dailyCommonsenses = args[0] as List<DailyCommonsense>;

            if (dailyCommonsenses.Count > 0)
                SetPage(0);
        }

        private void SetPage(int page)
        {
            wordText.text = dailyCommonsenses[page].word;
            meaningText.text = dailyCommonsenses[page].meaning;
            dateText.text = dailyCommonsenses[page].date.Insert(4,"/").Insert(7, "/");

            ContentSizeUpdateed();

            nextButton.interactable = page + 1 < MaxPage;
            prevButton.interactable = page - 1 > 0;

        }

        public void OnNextButtonClick()
        {
            SetPage(page + 1);
        }

        public void OnPrevButtonClick()
        {
            SetPage(page - 1);
        }

        public void OnZoomButtonClick()
        {
            isZoomed = !isZoomed;

            if (isZoomed)
                OnZoomText();
            else
                OffZoomText();
        }

        private void ContentSizeUpdateed()
        {
            meaningText.rectTransform.sizeDelta = meaningText.rectTransform.sizeDelta.NewY(meaningText.preferredHeight);

        }

        private void OnZoomText()
        {
            meaningText.fontSize = ZOOM_FONT_SIZE;
            ContentSizeUpdateed();
            zoomImageSwap.SetSprite(false);
        }

        private void OffZoomText()
        {
            meaningText.fontSize = BASIC_FONT_SIZE;
            ContentSizeUpdateed();
            zoomImageSwap.SetSprite(true);
        }
    }
}