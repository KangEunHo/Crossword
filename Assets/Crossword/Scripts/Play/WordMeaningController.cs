using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HealingJam.Crossword
{
    public class WordMeaningController : MonoBehaviour
    {
        [SerializeField] private Text typeText = null;
        [SerializeField] private Text meaningText = null;
        [SerializeField] private RectTransform content = null;

        public void OnCellBoardClick(object sender, BoardClickEvent boardClickEvent)
        {
            BoardCell boardCell = boardClickEvent.boardCell;
            WordDataForGame.Direction direction = boardClickEvent.direction;

            if (direction == WordDataForGame.Direction.None)
            {
                SetText(null);
            }
            else if (direction == WordDataForGame.Direction.Horizontal)
            {
                SetText(boardCell.HorizontalWordData);
            }
            else if (direction == WordDataForGame.Direction.Vertical)
            {
                SetText(boardCell.VerticalWordData);
            }
        }

        public void SetText(WordData wordData)
        {
            if (wordData == null)
            {
                typeText.text = "";
                meaningText.text = "";
                return;
            }
            typeText.text = "[" + WordTypeToString(wordData.wordType) + "]";
            meaningText.text = wordData.info;

            meaningText.rectTransform.sizeDelta = new Vector2(meaningText.rectTransform.sizeDelta.x, meaningText.preferredHeight);

            content.anchoredPosition = content.anchoredPosition.NewY(0f);
        }


        private string WordTypeToString(WordData.WordType wordType)
        {
            switch (wordType)
            {
                case WordData.WordType.Total:
                    break;
                case WordData.WordType.Humanities:
                    return "인문";
                case WordData.WordType.Term:
                    return "용어";
                case WordData.WordType.GeneralCommonSense:
                    return "일반상식";
                case WordData.WordType.CurrentAffairsCommonSense:
                    return "시사상식";
                case WordData.WordType.Idiom:
                    return "사자성어,속담";
                case WordData.WordType.Culture:
                    return "문화(미디어)";
                case WordData.WordType.History:
                    return "역사";
                case WordData.WordType.Science:
                    return "과학,IT,수학";
                case WordData.WordType.Politics:
                    return "정치,법률";
                case WordData.WordType.Economy:
                    return "경제,경영";
                case WordData.WordType.Max:
                    break;
                default:
                    return "";
            }
            return "";
        }
    }
}