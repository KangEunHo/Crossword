using System.Collections.Generic;

namespace HealingJam.Crossword.Save
{
    // 맞춘 갯수랑 문제갯수 데이터.
    public class RightAnswerCountData
    {
        public int rightAnswerCount;
        public int answerCount;
    }

    public class ProgressData
    {
        public List<string> machedWords = new List<string>();
    }

    public class LevelData
    {
        public bool completed = false;
        public List<RightAnswerCountData> rightAnswerCountDatas = new List<RightAnswerCountData>();
    }

    public class SaveData
    {
        public Dictionary<int, ProgressData> progressDatas = new Dictionary<int, ProgressData>();
        public List<bool> completeDatas = new List<bool>();
        public List<LevelData> levelDatas = new List<LevelData>();
    }
}