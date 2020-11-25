using UnityEngine;
using System.Collections.Generic;

namespace HealingJam.Crossword
{
    public static class LetterDatabase
    {
        public static List<char> database;

        public static void SetUpDatabase(params CrosswordMap[] crosswordMaps)
        {
            database = new List<char>();
            HashSet<char> containsChecker = new HashSet<char>();

            foreach (var crosswordMap in crosswordMaps)
            {
                foreach (var word in crosswordMap.wordDatas)
                {
                    for (int i = 0; i < word.word.Length; ++i)
                    {
                        char character = word.word[i];
                        if (containsChecker.Contains(character) == false)
                        {
                            database.Add(character);
                            containsChecker.Add(character);
                        }
                    }
                }
            }
        }

        public static char GetRandomValue()
        {
            return database.RandomValue();
        }
    }
}