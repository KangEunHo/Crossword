using System.Collections.Generic;

using HealingJam.Crossword;
namespace HealingJam.Crossword.Save
{
    public class ProgressData
    {
        public float elapsedTime;
        public Dictionary<WordData.WordType, int> wrongCountTypes = new Dictionary<WordData.WordType, int>();
        public List<string> machedWords = new List<string>();
    }

    public class CompleteData
    {
        public bool perfectClear;
    }

    public class SaveData
    {
        public Dictionary<int, ProgressData> progressDatas = new Dictionary<int, ProgressData>();
        public List<CompleteData> completeDatas = new List<CompleteData>();
        public int unlockPackLevel = 0;
    }
}