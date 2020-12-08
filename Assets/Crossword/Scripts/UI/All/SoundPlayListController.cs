using UnityEngine;
using System.Collections;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword
{
    [RequireComponent(typeof(SoundPlayList))]
    public class SoundPlayListController : MonoBehaviour
    {
        [SerializeField] private AudioClip bgm1 = null;
        [SerializeField] private AudioClip bgm2 = null;

        private SoundPlayList soundPlayList = null;

        private void Start()
        {
            soundPlayList = GetComponent<SoundPlayList>();

            OnUseBgm1Changed(SettingMgr.UseBgm1);
            OnUseBgm2Changed(SettingMgr.UseBgm2);

            SettingMgr.UseBgm1ChangedAction += OnUseBgm1Changed;
            SettingMgr.UseBgm2ChangedAction += OnUseBgm2Changed;
        }

        private void OnDestroy()
        {
            SettingMgr.UseBgm1ChangedAction -= OnUseBgm1Changed;
            SettingMgr.UseBgm2ChangedAction -= OnUseBgm2Changed;
        }

        public void OnUseBgm1Changed(bool value)
        {
            if (value)
                soundPlayList.AddSound(bgm1);
            else
                soundPlayList.RemoveSound(bgm1);
        }

        public void OnUseBgm2Changed(bool value)
        {
            if (value)
                soundPlayList.AddSound(bgm2);
            else
                soundPlayList.RemoveSound(bgm2);
        }
    }
}