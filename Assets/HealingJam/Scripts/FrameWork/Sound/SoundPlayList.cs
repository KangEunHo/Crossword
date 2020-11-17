using UnityEngine;
using System.Collections.Generic;

public class SoundPlayList : MonoBehaviour
{
    #region Member Variables

    private List<AudioClip> playList = null;
    private AudioSource audioSource = null;

    #endregion

    #region Properties

    public bool IsPlaying { get; private set; } = false;

    #endregion

    #region Initialize Methods

    public void Initialize(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        if (this.audioSource == null)
        {
            this.audioSource = this.gameObject.AddComponent<AudioSource>();
        }

        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;

        playList = new List<AudioClip>();
    }

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (IsPlaying && audioSource.clip != null)
        {
            if (audioSource.isPlaying == false && audioSource.isActiveAndEnabled)
            {
                audioSource.clip = GetNextSound();
                audioSource.Play();
            }
        }
    }

    #endregion

    #region Public Methods

    public void AddSound(AudioClip clip)
    {
        if (playList.Contains(clip))
            return;

        playList.Add(clip);
        if (audioSource.clip == null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            IsPlaying = true;
        }
    }

    public void RemoveSound(AudioClip clip)
    {
        if (playList.Contains(clip) == false)
            return;

        playList.Remove(clip);
        if (audioSource.clip == clip)
        {
            audioSource.clip = GetNextSound();
            if (audioSource.clip == null)
                IsPlaying = false;
            else
                audioSource.Play();
        }
    }

    #endregion

    #region Private Methods

    private AudioClip GetNextSound()
    {
        if (playList.Count == 0)
            return null;

        if (audioSource.clip == null)
            return playList[0];

        int nextIndex = playList.IndexOf(audioSource.clip) + 1;
        return playList[nextIndex % playList.Count];
    }

    #endregion
}
