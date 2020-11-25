using UnityEngine;
using System.Collections.Generic;

namespace HealingJam.Crossword
{
    public class CrosswordMap
    {
        public int mapSizeX;
        public int mapSizeY;
        public List<WordDataForGame> wordDatas = new List<WordDataForGame>();

        public WordDataForGame GetWordData(string word)
        {
            foreach(var v in wordDatas)
            {
                if (v.word == word)
                    return v;
            }
            return null;
        }

        public List<WordDataForGame> GetWordDataToSameTpye(WordData.WordType wordType)
        {
            return wordDatas.FindAll(wordData => wordData.wordType == wordType);
        }
    }
}