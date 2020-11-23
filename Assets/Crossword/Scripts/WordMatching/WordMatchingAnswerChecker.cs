using UnityEngine;
using System.Collections.Generic;

namespace HealingJam.Crossword
{
    public class WordMatchingAnswerChecker
    {
        private readonly int MAX_ANSWER_COUNT;

        private List<WordData> answers = null;
        public int MatchingCount { get; private set; }

        public WordMatchingAnswerChecker(List<WordData> answers)
        {
            MAX_ANSWER_COUNT = answers.Count;

            this.answers = answers;
        }


        /// <summary>
        /// 맞춘단어 목록에 단어를 추가합니다.
        /// </summary>
        /// <returns>모두 맞췄을시 true 아닐시 false 를 반환합니다. </returns>
        public bool AddMatchedWord(WordData wordDataForGame)
        {
            // 클리어.
            if (++MatchingCount >= MAX_ANSWER_COUNT)
            {
                return true;
            }
            return false;
        }

        public WordData GetUnMatchedWord()
        {
            return answers[MatchingCount];
        }

    }
}