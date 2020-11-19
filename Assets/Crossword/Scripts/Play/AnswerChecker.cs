using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace HealingJam.Crossword
{
    public class AnswerChecker
    {
        private readonly int MAX_ANSWER_COUNT;

        private HashSet<WordDataForGame> matchedWords = null;
        private HashSet<WordDataForGame> unmatchedWords = null;

        public AnswerChecker(CrosswordMap crosswordMap, BoardController boardController)
        {
            MAX_ANSWER_COUNT = crosswordMap.wordDatas.Count;

            matchedWords = new HashSet<WordDataForGame>();
            unmatchedWords = new HashSet<WordDataForGame>();

            foreach (var word in crosswordMap.wordDatas)
            {
                if (boardController.IsCompletedWord(word))
                {
                    matchedWords.Add(word);
                }
                else
                {
                    unmatchedWords.Add(word);
                }
            }
        }

        /// <summary>
        /// 맞춘단어 목록에 단어를 추가합니다.
        /// </summary>
        /// <returns>모두 맞췄을시 true 아닐시 false 를 반환합니다. </returns>
        public bool AddMatchedWord(WordDataForGame wordDataForGame)
        {
            unmatchedWords.Remove(wordDataForGame);
            matchedWords.Add(wordDataForGame);

            // 클리어.
            if (matchedWords.Count >= MAX_ANSWER_COUNT)
            {
                return true;
            }
            return false;
        }

        public bool IsMatchedWord(WordDataForGame word)
        {
            return matchedWords.Contains(word);
        }

        public WordDataForGame GetUnMatchedWord()
        {
            if (unmatchedWords.Count == 0)
                return null;

            return unmatchedWords.ToArray()[0];
        }

    }
}