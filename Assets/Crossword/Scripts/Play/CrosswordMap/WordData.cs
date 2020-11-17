using UnityEngine;
using System.Collections;

namespace HealingJam.Crossword
{
    public class WordData
    {
        public enum WordType : byte
        {
            Total = 0, Humanities = 1, Term = 2, GeneralCommonSense = 3, CurrentAffairsCommonSense = 4, Idiom = 5, Culture = 6, History = 7, Science = 8, Politics = 9, Economy = 10, Max = 11
        }
        public string word;
        public string info;
        public WordType wordType;
    }

    // 게임 플레이에 쓰이는 클래스.
    public class WordDataForGame : WordData
    {
        public enum Direction : byte
        {
            None = 0, Horizontal = 1, Vertical = 2, All = 3
        }
        public int x, y;
        public Direction direction;
    }
}