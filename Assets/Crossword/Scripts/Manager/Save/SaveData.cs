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
        public List<string> matchedWords = new List<string>();
    }

    public class LevelData
    {
        public bool completed = false;
        public List<RightAnswerCountData> rightAnswerCountDatas = new List<RightAnswerCountData>();
    }

    public class SaveData
    {
        public enum LoginType : int
        {
            None = 0, Guest =1, Google =2
        }

        public List<bool> completeDatas = new List<bool>();
        public List<LevelData> levelDatas = new List<LevelData>();
        public int coin = 200;
        public bool isAdRemoved = false;
        public bool playedCommonSenseTest = false;

        public LoginType loginType = LoginType.None;

        public int LastUnlockPackIndex()
        {
            for (int i = 0; i < completeDatas.Count; ++i)
            {
                if (completeDatas[i] == false)
                    return i;
            }
            return completeDatas.Count;
        }

        public int GetUnlockLevel()
        {
            for (int i = 0; i < levelDatas.Count; ++i)
            {
                if (levelDatas[i].completed == false)
                    return i;
            }
            return levelDatas.Count;
        }
    }
}