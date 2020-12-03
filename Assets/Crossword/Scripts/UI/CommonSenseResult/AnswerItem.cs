using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HealingJam.Crossword.UI
{
    public class AnswerItem : MonoBehaviour
    {
        public class AnswerItemData
        {
            public WordData wordData;
            public bool correctAnswer;
        }
        [SerializeField] private ImageSwap oxImageSwap = null;
        [SerializeField] private Text wordText = null;

        private AnswerItemData answerItemData;

        public void SetUp(AnswerItemData answerItemData)
        {
            this.answerItemData = answerItemData;

            wordText.text = answerItemData.wordData.word;
            oxImageSwap.SetSprite(answerItemData.correctAnswer);
        }

        public void OnPlusButtonClikck()
        {

        }
    }
}