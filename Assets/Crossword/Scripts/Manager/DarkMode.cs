using UnityEngine;
using System;
using System.Collections;

namespace HealingJam.Crossword
{
    public static class DarkMode
    {
        public static event Action<bool> DrakModeChangeAction = null;

        private static bool useDarkMode = false;
        public static bool UseDarkMode { get { return useDarkMode; } set {
                useDarkMode = value;
                DrakModeChangeAction?.Invoke(value);
            } }
    }
}