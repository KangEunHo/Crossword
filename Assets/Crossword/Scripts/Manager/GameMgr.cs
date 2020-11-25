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

            SetUpDatabase();

            ScreenMgr.Instance.Enter(GameScreen.ScreenID.Title);

        }

        private void SetUpDatabase()
        {
            int maxStage = CrosswordMapManager.Instance.MaxStage();
            CrosswordMap[] crosswordMaps = new CrosswordMap[maxStage];
            for (int i = 0; i < maxStage; ++i)
            {
                crosswordMaps[i] = CrosswordMapManager.Instance.GetCrosswordMap(i);
            }
            LetterDatabase.SetUpDatabase(crosswordMaps);
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
    }
}