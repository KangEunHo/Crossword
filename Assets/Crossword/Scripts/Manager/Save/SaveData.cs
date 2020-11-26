using System.Collections.Generic;

namespace HealingJam.Crossword.Save
{
    public class ProgressData
    {
        public List<string> machedWords = new List<string>();
    }

    public class LevelData
    {
        public bool completed = false;
        public Dictionary<WordData.WordType, int> wrongCountsByType = new Dictionary<WordData.WordType, int>();
    }

    public class SaveData
    {
        public Dictionary<int, ProgressData> progressDatas = new Dictionary<int, ProgressData>();
        public List<bool> completeDatas = new List<bool>();
        public List<LevelData> levelDatas = new List<LevelData>();
        public int unlockPack = 0;
    }
}