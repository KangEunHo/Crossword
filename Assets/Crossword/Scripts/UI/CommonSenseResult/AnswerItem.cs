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

        private int index = 0;
        private AnswerItemData answerItemData;

        public System.Action<int> PlusButtonClickAction { get; set; } = null;

        public void SetUp(int index, AnswerItemData answerItemData)
        {
            this.answerItemData = answerItemData;
            this.index = index;

            wordText.text = answerItemData.wordData.word;
            oxImageSwap.SetSprite(answerItemData.correctAnswer);
        }

        public void OnPlusButtonClik()
        {
            PlusButtonClickAction?.Invoke(index);
        }
    }
}