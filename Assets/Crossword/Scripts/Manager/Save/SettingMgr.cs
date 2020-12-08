using UnityEngine;
using System;

namespace HealingJam.Crossword.Save
{
    public static class SettingMgr
    {
        public static event Action<bool> UseBgm1ChangedAction = null;
        public static event Action<bool> UseBgm2ChangedAction = null;
        public static event Action<bool> UseEfxChangedAction = null;
        public static event Action<bool> UseVibrationChangedAction = null;
        public static event Action<bool> UseDarkModeChangedAction = null;

        public static bool UseBgm1 { get; private set; }
        public static bool UseBgm2 { get; private set; }
        public static bool UseEfx { get; private set; }
        public static bool UseVibration { get; private set; }
        public static bool UseDarkMode { get; private set; }

        static SettingMgr()
        {
            LoadAtLocal();
        }

        private static void LoadAtLocal()
        {
            UseBgm1 = PlayerPrefs.GetInt("use_bgm1", 1) == 1;
            UseBgm2 = PlayerPrefs.GetInt("use_bgm2", 1) == 1;
            UseEfx = PlayerPrefs.GetInt("use_Efx", 1) == 1;
            UseVibration = PlayerPrefs.GetInt("use_vibration", 1) == 1;
            UseDarkMode = PlayerPrefs.GetInt("use_dark_mode", 0) == 1;

        }

        public static void SaveToLocal()
        {
            PlayerPrefs.SetInt("use_bgm", UseBgm1 ? 1 : 0);
            PlayerPrefs.SetInt("use_bgm2", UseBgm2 ? 1 : 0);
            PlayerPrefs.SetInt("use_Efx", UseEfx ? 1 : 0);
            PlayerPrefs.SetInt("use_vibration", UseVibration ? 1 : 0);
            PlayerPrefs.SetInt("use_dark_mode", UseDarkMode ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void SetUseBgm1(bool value)
        {
            UseBgm1 = value;
            UseBgm1ChangedAction?.Invoke(value);
        }

        public static void SetUseBgm2(bool value)
        {
            UseBgm2 = value;
            UseBgm2ChangedAction?.Invoke(value);
        }

        public static void SetUseEfx(bool value)
        {
            UseEfx = value;
            UseEfxChangedAction?.Invoke(value);
        }

        public static void SetUseVibration(bool value)
        {
            UseVibration = value;
            UseVibrationChangedAction?.Invoke(value);
        }

        public static void SetUseDarkMode(bool value)
        {
            UseDarkMode = value;
            UseDarkModeChangedAction?.Invoke(value);
        }
    }
}