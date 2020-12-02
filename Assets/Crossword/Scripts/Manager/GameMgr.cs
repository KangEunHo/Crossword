using UnityEngine;
using System.Collections;
using HealingJam.GameScreens;
using HealingJam.Crossword.Save;
using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class GameMgr : MonoSingleton<GameMgr>
    {

        private IEnumerator Start()
        {
            SaveMgr.Instance.Load();

            yield return StartCoroutine(CrosswordMapManager.Instance.LoadCrosswordMapAtAssetBundle());

            SetUpDatabase();

            DailyCommonsensePopup dailyCommonsensePopup = PopupMgr.Instance.GetPopupById(Popup.PopupID.DailyCommonSense) as DailyCommonsensePopup;

            yield return StartCoroutine(dailyCommonsensePopup.LoadCommonSenseAsync());

            ScreenMgr.Instance.Enter(GameScreen.ScreenID.Title);

        }

        private void SetUpDatabase()
        {
            int maxStage = CrosswordMapManager.Instance.MaxStage();

            Debug.Log(maxStage);

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