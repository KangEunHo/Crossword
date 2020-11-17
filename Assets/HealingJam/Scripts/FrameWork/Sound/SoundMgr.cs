using UnityEngine;
using System.Collections;

namespace HealingJam
{
    public class SoundMgr : MonoSingleton<SoundMgr>
    {
        #region Const Variables

        private const string MUSIC_PREF_KEY = "MusicPreference";
        private const int MUSIC_OFF = 0;
        private const int MUSIC_ON = 1;
        private const string SOUND_PREF_KEY = "SoundPreference";
        private const int SOUND_OFF = 0;
        private const int SOUND_ON = 1;

        #endregion

        #region Definition

        [System.Serializable]
        public class Sound
        {
            public AudioClip clip;
            [HideInInspector]
            public int simultaneousPlayCount = 1;
        }

        public delegate void MusicStatusChangedHandler(bool isOn);
        public delegate void SoundStatusChangedHandler(bool isOn);

        enum PlayingState
        {
            Playing, Paused, Stopped
        }

        #endregion

        #region Inspector Variables

        [Header("Max number allowed of same sounds playing together")]
        public int maxSimultaneousSounds = 7;

        #endregion

        #region Public Variables

        // List of sounds used in this game

        [Header("BGM list")]
        public Sound bgm;

        [Header("Sound list")]
        public Sound efx;

        //
        public event MusicStatusChangedHandler MusicStatusChanged;
        public event SoundStatusChangedHandler SoundStatusChanged;

        #endregion

        #region Member Variables

        private PlayingState musicState = PlayingState.Stopped;

        private AudioSource[] bgmSource;
        private AudioSource sfxSource;

        private int activeBgmSourceIndex = 0;
        private AudioSource ActiveBgmSource { get { return bgmSource[activeBgmSourceIndex]; } }
        private AudioSource UnActiveBgmSource { get { return bgmSource[1 - activeBgmSourceIndex]; } }

        float efxVolum = 1f;
        #endregion

        #region Initialize Methods

        public override void Init()
        {
            bgmSource = new AudioSource[2];

            for (int i = 0; i < bgmSource.Length; i++)
            {
                var obj = new GameObject("Bgm Source " + (i + 1));
                bgmSource[i] = obj.AddComponent<AudioSource>();
                bgmSource[i].loop = true;
                bgmSource[i].spatialBlend = 0f;
                obj.transform.SetParent(transform);
            }

            var sfxObj = new GameObject("Sfx source");
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.spatialBlend = 0f;
            sfxObj.transform.SetParent(transform);

            SetMusicOn(IsMusicON());
            SetSoundOn(IsSoundOn());
        }

        #endregion

        #region Public Methods

        public void PlayOneShot(Sound sound)
        {
            sfxSource.PlayOneShot(sound.clip, efxVolum);
        }

        public void PlayOneShot(AudioSource source, Sound sound)
        {
            if (IsMusicON())
                source.PlayOneShot(sound.clip, efxVolum);
        }
        /// <summary>
        /// Plays the given sound with option to progressively scale down volume of multiple copies of same sound playing at
        /// the same time to eliminate the issue that sound amplitude adds up and becomes too loud.
        /// </summary>
        /// <param name="sound">Sound.</param>
        /// <param name="autoScaleVolume">If set to <c>true</c> auto scale down volume of same sounds played together.</param>
        /// <param name="maxVolumeScale">Max volume scale before scaling down.</param>
        public void PlayAutoVolume(Sound sound)
        {
            StartCoroutine(CR_PlayAutoVolume(sound));
        }

        /// <summary>
        /// Plays the given music.
        /// </summary>
        /// <param name="music">Music.</param>
        /// <param name="loop">If set to <c>true</c> loop.</param>
        public void PlayMusic(Sound music, bool loop = true)
        {
            if (musicState == PlayingState.Playing)
            {
                if (ActiveBgmSource.clip != null && ActiveBgmSource.clip.name == music.clip.name)
                    return;
            }
            else if (musicState == PlayingState.Paused)
            {
                ActiveBgmSource.UnPause();
            }

            ActiveBgmSource.clip = music.clip;
            ActiveBgmSource.loop = loop;
            ActiveBgmSource.Play();
            musicState = PlayingState.Playing;
        }

        public void PlayCrossFadeMusic(Sound music, float fadeDuration = 1f)
        {
            if (ActiveBgmSource.clip != null && ActiveBgmSource.clip.name == music.clip.name)
                return;

            if (musicState == PlayingState.Paused)
            {
                ActiveBgmSource.UnPause();
                UnActiveBgmSource.UnPause();
            }

            activeBgmSourceIndex = 1 - activeBgmSourceIndex;
            ActiveBgmSource.clip = music.clip;

            ActiveBgmSource.Play();

            StartCoroutine(CR_CrossFadeMusic(fadeDuration));
        }

        /// <summary>
        /// Pauses the music.
        /// </summary>
        public void PauseMusic()
        {
            if (musicState == PlayingState.Playing)
            {
                ActiveBgmSource.Pause();
                UnActiveBgmSource.Pause();
                musicState = PlayingState.Paused;
            }
        }

        /// <summary>
        /// Resumes the music.
        /// </summary>
        public void ResumeMusic()
        {
            if (musicState == PlayingState.Paused)
            {
                ActiveBgmSource.UnPause();
                UnActiveBgmSource.UnPause();
                musicState = PlayingState.Playing;
            }
        }

        /// <summary>
        /// Stop music.
        /// </summary>
        public void StopMusic()
        {
            ActiveBgmSource.Stop();
            ActiveBgmSource.clip = null;
            UnActiveBgmSource.Stop();
            UnActiveBgmSource.clip = null;
            musicState = PlayingState.Stopped;
        }

        public void StopSound()
        {
            sfxSource.Stop();
        }

        /// <summary>
        /// Toggles the sound status.
        /// </summary>
        public void ToggleSound()
        {
            SetSoundOn(!IsSoundOn());
        }

        public bool IsSoundOn()
        {
            return PlayerPrefs.GetInt(SOUND_PREF_KEY, SOUND_ON) == SOUND_ON;
        }

        public void SetSoundOn(bool isOn)
        {
            int lastStatus = PlayerPrefs.GetInt(SOUND_PREF_KEY, SOUND_ON);
            int status = isOn ? 1 : 0;

            PlayerPrefs.SetInt(SOUND_PREF_KEY, status);
            sfxSource.mute = !isOn;

            if (lastStatus != status)
            {
                SoundStatusChanged?.Invoke(isOn);
            }
        }

        public bool IsMusicON()
        {
            return (PlayerPrefs.GetInt(MUSIC_PREF_KEY, MUSIC_ON) == MUSIC_ON);
        }
        /// <summary>
        /// Toggles the mute status.
        /// </summary>
        public void ToggleMusic()
        {
            if (IsMusicON())
            {
                // Turn music OFF
                PlayerPrefs.SetInt(MUSIC_PREF_KEY, MUSIC_OFF);
                if (musicState == PlayingState.Playing)
                {
                    PauseMusic();
                }

                MusicStatusChanged?.Invoke(false);
                ActiveBgmSource.mute = true;
                UnActiveBgmSource.mute = true;
            }
            else
            {
                // Turn music ON
                PlayerPrefs.SetInt(MUSIC_PREF_KEY, MUSIC_ON);
                if (musicState == PlayingState.Paused)
                {
                    ResumeMusic();
                }

                MusicStatusChanged?.Invoke(true);
                ActiveBgmSource.mute = false;
                UnActiveBgmSource.mute = false;
            }
        }

        public void SetMusicOn(bool isOn)
        {
            int lastStatus = PlayerPrefs.GetInt(MUSIC_PREF_KEY, MUSIC_ON);
            int status = isOn ? 1 : 0;

            PlayerPrefs.SetInt(MUSIC_PREF_KEY, status);
            ActiveBgmSource.mute = !isOn;
            UnActiveBgmSource.mute = !isOn;

            if (lastStatus != status)
            {
                MusicStatusChanged?.Invoke(isOn);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator CR_PlayAutoVolume(Sound sound)
        {
            if (sound.simultaneousPlayCount >= maxSimultaneousSounds)
            {
                yield break;
            }

            sound.simultaneousPlayCount++;

            float vol = 1f;

            // Scale down volume of same sound played subsequently
            if (sound.simultaneousPlayCount > 1)
            {
                vol = vol / (float)(sound.simultaneousPlayCount);
            }

            sfxSource.PlayOneShot(sound.clip, vol);

            // Wait til the sound almost finishes playing then reduce play count
            float delay = sound.clip.length * 0.7f;

            yield return new WaitForSeconds(delay);

            sound.simultaneousPlayCount--;
        }

        private IEnumerator CR_CrossFadeMusic(float duration)
        {
            float percent = 0f;

            while (percent < 1f)
            {
                percent += Time.deltaTime * (1 / duration);

                ActiveBgmSource.volume = Mathf.Lerp(0, 1f, percent);

                UnActiveBgmSource.volume = Mathf.Lerp(1f, 0, percent);

                yield return null;
            }
            UnActiveBgmSource.Stop();
        }
        #endregion

    }
}