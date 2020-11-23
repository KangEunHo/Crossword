using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword
{
    public class AnswerChecker
    {
        private readonly int MAX_ANSWER_COUNT;

        private HashSet<string> matchedWords = null;
        private Dictionary<string, WordDataForGame> unmatchedWords = null;

        public AnswerChecker(List<WordDataForGame> wordDataForGames, BoardController boardController)
        {
            MAX_ANSWER_COUNT = wordDataForGames.Count;

            matchedWords = new HashSet<string>();
            unmatchedWords = new Dictionary<string, WordDataForGame>();

            foreach (var word in wordDataForGames)
            {
                if (boardController != null && boardController.IsCompletedWord(word))
                {
                    matchedWords.Add(word.word);
                }
                else
                {
                    unmatchedWords.Add(word.word, word);
                }
            }
        }


        /// <summary>
        /// 맞춘단어 목록에 단어를 추가합니다.
        /// </summary>
        /// <returns>모두 맞췄을시 true 아닐시 false 를 반환합니다. </returns>
        public bool AddMatchedWord(string word)
        {
            unmatchedWords.Remove(word);
            matchedWords.Add(word);

            // 클리어.
            if (matchedWords.Count >= MAX_ANSWER_COUNT)
            {
                return true;
            }
            return false;
        }

        public bool IsMatchedWord(WordDataForGame word)
        {
            return matchedWords.Contains(word.word);
        }

        public WordDataForGame GetUnMatchedWord()
        {
            if (unmatchedWords.Count == 0)
                return null;

            return unmatchedWords.Values.First();
        }

    }
}