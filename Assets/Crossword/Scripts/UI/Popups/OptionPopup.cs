using UnityEngine;
using System.Collections;
using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class OptionPopup : CallbackPopup
    {
        [SerializeField] private ToggleController bgmToggleController = null;
        [SerializeField] private ToggleController efxToggleController = null;
        [SerializeField] private ToggleController vibrationToggleController = null;
        [SerializeField] private ToggleController darkModeToggleController = null;

        public override void Init(object stateMachine)
        {
            bgmToggleController.SetPosition(SoundMgr.Instance.IsMusicON());
            efxToggleController.SetPosition(SoundMgr.Instance.IsSoundOn());
            vibrationToggleController.SetPosition(true);
            darkModeToggleController.SetPosition(DarkMode.UseDarkMode);

            bgmToggleController.valueChangeAction += OnBgmToggle;
            efxToggleController.valueChangeAction += OnSoundToggle;
            vibrationToggleController.valueChangeAction += OnVibrationToggle;
            darkModeToggleController.valueChangeAction += OnDarkModeToggle;
        }

        public void OnBgmToggle(bool isOn)
        {
            SoundMgr.Instance.SetMusicOn(isOn);
        }

        public void OnSoundToggle(bool isOn)
        {
            SoundMgr.Instance.SetSoundOn(isOn);
        }

        public void OnVibrationToggle(bool isOn)
        {

        }

        public void OnDarkModeToggle(bool isOn)
        {
            DarkMode.UseDarkMode = isOn;
        }
    }
}