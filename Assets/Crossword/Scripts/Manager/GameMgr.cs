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
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Title);
            if (Input.GetKeyDown(KeyCode.B))
                ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Statistics);
            if (Input.GetKeyDown(KeyCode.C))
                ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.StageSelect);
            if (Input.GetKeyDown(KeyCode.D))
                ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Clear);
            if (Input.GetKeyDown(KeyCode.E))
                ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Option);
        }
    }
}