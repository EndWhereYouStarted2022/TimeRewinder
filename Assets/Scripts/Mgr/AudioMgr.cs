using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DFramework
{
    public class AudioMgr : MonoSingleton<AudioMgr>
    {
        private AudioListener mAudioListener;
        private AudioSource mMusicSource;

        private AudioMgr() { }

        public void CheckAudioListener()
        {
            if (!mAudioListener)
            {
                mAudioListener = gameObject.AddComponent<AudioListener>();
            }
        }

        public void PlaySound(string soundName)
        {
            CheckAudioListener();
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>(soundName);
            audioSource.Play();
        }

        public void PlayMusic(string musicName, bool loop = true)
        {
            CheckAudioListener();
            if (!mMusicSource)
            {
                mMusicSource = gameObject.AddComponent<AudioSource>();
            }
            mMusicSource.Stop();
            mMusicSource.clip = Resources.Load<AudioClip>(musicName);
            mMusicSource.loop = loop;
            mMusicSource.Play();
        }

        public void StopMusic()
        {
            if (mMusicSource)
            {
                mMusicSource.Stop();
            }
        }

        public void PauseMusic()
        {
            if (mMusicSource)
            {
                mMusicSource.Pause();
            }
        }

        public void ResumeMusic()
        {
            if (mMusicSource)
            {
                mMusicSource.UnPause();
            }
        }

        public void MusicOn()
        {
            if (mMusicSource)
            {
                mMusicSource.UnPause();
                mMusicSource.mute = false;
            }
        }

        public void MusicOff()
        {
            if (mMusicSource)
            {
                mMusicSource.Pause();
                mMusicSource.mute = true;
            }
        }

        public void SoundOn()
        {
            var audioSources = gameObject.GetComponents<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                if (audioSource != mMusicSource)
                {
                    audioSource.UnPause();
                    audioSource.mute = false;
                }
            }
        }

        public void SoundOff()
        {
            var audioSources = gameObject.GetComponents<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                if (audioSource != mMusicSource)
                {
                    audioSource.Pause();
                    audioSource.mute = true;
                }
            }
        }
    }

}
