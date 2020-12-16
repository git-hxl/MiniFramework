using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private AudioSource musicSource;
        private AudioSource soundSource;

        private float totalVolume = 1f;
        private float musicVolume = 1f;
        private float soundVolume = 1f;
        protected override void Awake()
        {
            base.Awake();
            musicSource = transform.Find("Music Source").GetComponent<AudioSource>();
            soundSource = transform.Find("Sound Source").GetComponent<AudioSource>();
            totalVolume = PlayerPrefs.GetFloat("TotalVolume", 1);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
            soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
            musicSource.volume = musicVolume * totalVolume;
            soundSource.volume = soundVolume * totalVolume;
        }
        public float GetTotalVolume
        {
            get { return totalVolume; }
        }
        public float GetSoundVolume
        {
            get { return soundVolume; }
        }
        public float GetMusicVolume
        {
            get { return musicVolume; }
        }
        /// <summary>
        /// 设置总音量大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetTotalVolume(float volume)
        {
            totalVolume = volume;
            soundSource.volume = soundVolume * totalVolume;
            musicSource.volume = musicVolume * totalVolume;
            PlayerPrefs.SetFloat("TotalVolume", volume);
        }

        /// <summary>
        /// 设置音乐音量大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
            musicSource.volume = volume * totalVolume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
        /// <summary>
        /// 设置音效音量大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundVolume(float volume)
        {
            soundVolume = volume;
            soundSource.volume = volume * totalVolume;
            PlayerPrefs.SetFloat("SoundVolume", volume);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="assetPath"></param>
        public void PlaySound(string assetPath)
        {
            AudioClip clip = ResourceManager.Instance.LoadAsset<AudioClip>(assetPath);
            soundSource.PlayOneShot(clip);
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="assetPath"></param>
        public void PlayMusic(string assetPath)
        {
            AudioClip clip = ResourceManager.Instance.LoadAsset<AudioClip>(assetPath);
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void Clear()
        {
            musicSource.clip = null;
            Resources.UnloadUnusedAssets();
        }

        public override void Dispose()
        {
            Clear();
            base.Dispose();
        }
    }
}