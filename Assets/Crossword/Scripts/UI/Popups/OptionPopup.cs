using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HealingJam.Popups;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword
{
    public class OptionPopup : CallbackPopup
    {
        [SerializeField] private Toggle bgm1Toggle = null;
        [SerializeField] private Toggle bgm2Toggle = null;

        [SerializeField] private ToggleController efxToggleController = null;
        [SerializeField] private ToggleController vibrationToggleController = null;
        [SerializeField] private ToggleController darkModeToggleController = null;

        public override void Init(object stateMachine)
        {
            bgm1Toggle.isOn = SettingMgr.UseBgm1;
            bgm2Toggle.isOn = SettingMgr.UseBgm2;
            bgm1Toggle.onValueChanged.AddListener(OnBgm1ValueChanged);
            bgm2Toggle.onValueChanged.AddListener(OnBgm2ValueChanged);


            efxToggleController.SetPosition(SoundMgr.Instance.IsSoundOn());
            vibrationToggleController.SetPosition(SettingMgr.UseVibration); ;
            darkModeToggleController.SetPosition(DarkMode.UseDarkMode);

            efxToggleController.valueChangeAction += OnSoundToggle;
            vibrationToggleController.valueChangeAction += OnVibrationToggle;
            darkModeToggleController.valueChangeAction += OnDarkModeToggle;
        }

        public void OnBgm1ValueChanged(bool isOn)
        {
            SettingMgr.SetUseBgm1(isOn);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnBgm2ValueChanged(bool isOn)
        {
            SettingMgr.SetUseBgm2(isOn);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnSoundToggle(bool isOn)
        {
            SoundMgr.Instance.SetSoundOn(isOn);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnVibrationToggle(bool isOn)
        {
            SettingMgr.SetUseVibration(isOn);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnDarkModeToggle(bool isOn)
        {
            SettingMgr.SetUseDarkMode(isOn);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnGoogleLoginButtonClick()
        {
            SaveMgr.Instance.SetLoginType(SaveData.LoginType.Google);
            Escape();
            GameScreens.ScreenMgr.Instance.ChangeState(GameScreens.GameScreen.ScreenID.Loading);

            ToastPlugin.ToastHelper.ShowToast("데이터를 불러오기위해 게임을 재시작합니다", true);
        }
    }
}