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
        private BoardController boardController = null;

        public AnswerChecker(List<WordDataForGame> wordDataForGames, BoardController boardController)
        {
            MAX_ANSWER_COUNT = wordDataForGames.Count;
            this.boardController = boardController;

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

            // 한 단어를 맞출시에 이어진 단어를 맞출 수도 있으니 맞춰진 단어가 있으면 맞춘목록에 추가합니다.
            if (boardController != null)
            {
                MatchCheckAllUnmachedWord();
            }
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

        public WordDataForGame GetUnMatchedWordOrderByRange(Vector2Int index)
        {
            if (unmatchedWords.Count == 0)
                return null;
            var list = unmatchedWords.Values.OrderBy(word => Mathf.Abs(word.x - index.x) + Mathf.Abs(word.y - index.y));
            return list.First();
        }

        public WordDataForGame GetUnMatchedWordByConnecte(WordDataForGame wordDataForGame, BoardController boardController)
        {
            if (unmatchedWords.Count == 0)
                return null;
            
            for (int i = 0; i < wordDataForGame.word.Length; ++i)
            {
                int x = wordDataForGame.x;
                int y = wordDataForGame.y;

                if (wordDataForGame.direction == WordDataForGame.Direction.Horizontal)
                    x += i;
                else y += i;

                BoardCell boardCell = boardController.GetBoardCell(new Vector2Int(x, y));
                if (wordDataForGame.direction == WordDataForGame.Direction.Horizontal)
                {
                    if (boardCell.VerticalWordData != null && unmatchedWords.ContainsKey(boardCell.VerticalWordData.word))
                    {
                        return boardCell.VerticalWordData;
                    }
                }
                else
                {
                    if (boardCell.HorizontalWordData != null && unmatchedWords.ContainsKey(boardCell.HorizontalWordData.word))
                    {
                        return boardCell.HorizontalWordData;
                    }
                }
            }
            return null;
        }

        private void MatchCheckAllUnmachedWord()
        {
            List<string> matchedWord = new List<string>();
            foreach(var word in unmatchedWords.Values)
            {
                if (boardController.IsCompletedWord(word))
                {
                    matchedWord.Add(word.word);
                }
            }

            foreach(var word in matchedWord)
            {
                AddMatchedWord(word);
            }
        }
    }
}