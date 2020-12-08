﻿using UnityEngine;
using System.Collections;
using HealingJam.GameScreens;
using HealingJam.Crossword.Save;
using HealingJam.Popups;
using HealingJam.Crossword.UI;

namespace HealingJam.Crossword
{
    public class GameMgr : MonoSingleton<GameMgr>
    {
        public TopUIController topUIController = null;

        protected override void Awake()
        {
            base.Awake();

            DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 150, sequencesCapacity: 100);
            DG.Tweening.DOTween.defaultAutoKill = true;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            SaveMgr.Instance.Load();
        }

        private void Start()
        {
            ScreenMgr.Instance.Enter(GameScreen.ScreenID.Title);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                DarkMode.UseDarkMode = true;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                DarkMode.UseDarkMode = false;
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveMgr.Instance.Save();
            }
        }

        protected override void OnApplicationQuit()
        {
            SaveMgr.Instance.Save();
        }
    }
}