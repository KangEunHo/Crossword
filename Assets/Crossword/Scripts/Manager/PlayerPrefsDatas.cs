using UnityEngine;

namespace HealingJam.Crossword
{
    public static class PlayerPrefsDatas 
    {
        public static bool GetBoolData(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue) == 0 ? false : true;
        }

        public static void SetBoolData(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value == false ? 0 : 1);
        }
    }
}