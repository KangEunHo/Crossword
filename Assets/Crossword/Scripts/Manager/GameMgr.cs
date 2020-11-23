using UnityEngine;
using System.Collections;
using HealingJam.GameScreens;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword
{
    public class GameMgr : MonoSingleton<GameMgr>
    {

        private void Start()
        {
            SaveMgr.Instance.Load();
            ScreenMgr.Instance.Enter(GameScreen.ScreenID.Title);
        }

    }
}