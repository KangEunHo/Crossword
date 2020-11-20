using System.Collections.Generic;

namespace HealingJam.Crossword.Save
{
    public class ProgressData
    {
        public List<string> machedWords = new List<string>();
        public float elapsedTime;
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