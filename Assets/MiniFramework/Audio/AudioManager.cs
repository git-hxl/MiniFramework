using UnityEngine;
using MiniFramework.Resource;
namespace MiniFramework.Audio
{
    public sealed class AudioManager : MonoSingleton<AudioManager>, IAudioManager
    {
        private AudioSource MusicSource;
        private AudioSource SoundSource;
        private float musicVolume;//音乐音量
        private float soundVolume;//音效音量
        private float totalVolume;//总音量

        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        private void Init()
        {
            GameObject music = new GameObject("MusicSource", typeof(AudioSource));
            music.transform.SetParent(transform);
            MusicSource = music.GetComponent<AudioSource>();
            MusicSource.loop = true;

            GameObject sound = new GameObject("SoundSource", typeof(AudioSource));
            sound.transform.SetParent(transform);
            SoundSource = sound.GetComponent<AudioSource>();
            SoundSource.loop = false;

            totalVolume = PlayerPrefs.GetFloat("TotalVolume", 1);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
            soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
            MusicSource.volume = musicVolume * totalVolume;
            SoundSource.volume = soundVolume * totalVolume;
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
            SoundSource.volume = soundVolume * totalVolume;
            MusicSource.volume = musicVolume * totalVolume;
            PlayerPrefs.SetFloat("TotalVolume", volume);
        }
        /// <summary>
        /// 设置音乐音量大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
            MusicSource.volume = volume * totalVolume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
        /// <summary>
        /// 设置音效音量大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundVolume(float volume)
        {
            soundVolume = volume;
            SoundSource.volume = volume * totalVolume;
            PlayerPrefs.SetFloat("SoundVolume", volume);
        }
        /// <summary>
        /// 静音
        /// </summary>
        public void DisableVolume()
        {
            SoundSource.volume = 0;
            MusicSource.volume = 0;
        }
        /// <summary>
        /// 恢复静音
        /// </summary>
        public void EnableVolume()
        {
            SoundSource.volume = soundVolume * totalVolume;
            MusicSource.volume = musicVolume * totalVolume;
        }
        /// <summary>
        /// 加载音效并播放
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public AudioSource PlaySound(string clipName, bool isOnShot = false)
        {
            AudioClip clip = ResourceManager.Instance.LoadAsset<AudioClip>(clipName);
            return PlaySound(clip, isOnShot);
        }
        /// <summary>
        /// 加载音效并播放
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public void PlaySoundAtPoint(string clipName, Vector3 pos)
        {
            AudioClip clip = ResourceManager.Instance.LoadAsset<AudioClip>(clipName);
            PlaySoundAtPoint(clip, pos);
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public AudioSource PlaySound(AudioClip audioClip, bool isOnShot = false)
        {
            if (isOnShot)
            {
                SoundSource.PlayOneShot(audioClip);
            }
            else
            {
                SoundSource.clip = audioClip;
                SoundSource.Play();
            }
            return SoundSource;
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public void PlaySoundAtPoint(AudioClip audioClip, Vector3 pos)
        {
            AudioSource.PlayClipAtPoint(audioClip, pos, SoundSource.volume);
        }
        /// <summary>
        /// 加载音乐并播放
        /// </summary>
        /// <param name="clipName"></param>
        /// <returns></returns>

        public AudioSource PlayMusic(string clipName)
        {
            AudioClip clip = ResourceManager.Instance.LoadAsset<AudioClip>(clipName);
            return PlayMusic(clip);
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip"></param>
        /// <returns></returns>
        public AudioSource PlayMusic(AudioClip audioClip)
        {
            MusicSource.clip = audioClip;
            MusicSource.Play();
            return MusicSource;
        }
        /// <summary>
        /// 暂停当前音乐
        /// </summary>
        public void StopCurMusic()
        {
            MusicSource.Stop();
        }
        /// <summary>
        /// 播放当前音乐
        /// </summary>
        public void PlayCurMusic()
        {
            MusicSource.Play();
        }
    }
}