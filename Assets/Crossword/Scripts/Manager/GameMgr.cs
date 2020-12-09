using UnityEngine;
using System.Collections;
using HealingJam.GameScreens;
using HealingJam.Crossword.Save;
using HealingJam.Popups;
using HealingJam.Crossword.UI;
using HealingJam;

namespace HealingJam.Crossword
{
    public class GameMgr : MonoSingleton<GameMgr>
    {
        public TopUIController topUIController = null;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_IOS
        string path = Application.persistentDataPath;
        UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif

            DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 150, sequencesCapacity: 100);
            DG.Tweening.DOTween.defaultAutoKill = true;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            SaveMgr.Instance.Load();

            OnDarkModeChanged(SettingMgr.UseDarkMode);
            OnVibrationChanged(SettingMgr.UseVibration);

            SettingMgr.UseDarkModeChangedAction += OnDarkModeChanged;
            SettingMgr.UseVibrationChangedAction += OnVibrationChanged;

            Vibration.Init();
        }

        private void Start()
        {
            ScreenMgr.Instance.Enter(GameScreen.ScreenID.Title);

            GoogleMobileAdsMgr.Instance.ShowBannerAD();
        }

#if UNITY_ANDROID
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PopupMgr.Instance.CloseOpenedPopup() == false)
                {
                    topUIController.OnBackButtonClick();
                }

            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                DarkMode.UseDarkMode = true;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                DarkMode.UseDarkMode = false;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SaveMgr.Instance.AddCoin(100);
            }
        }
#endif

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveMgr.Instance.Save();
                SettingMgr.SaveToLocal();
            }
        }

        protected override void OnApplicationQuit()
        {
            SaveMgr.Instance.Save();
            SettingMgr.SaveToLocal();
        }

        private void OnDarkModeChanged(bool value)
        {
            DarkMode.UseDarkMode = value;
        }

        private void OnVibrationChanged(bool value)
        {

        }
    }
}