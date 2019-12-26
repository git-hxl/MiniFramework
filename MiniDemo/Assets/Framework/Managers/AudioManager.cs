using UnityEngine;
namespace MiniFramework
{
    public class AudioManager : MonoSingleton<AudioManager>
    {

        private AudioSource MusicSource;
        private AudioSource SoundSource;
        private float musicVolume;
        private float soundVolume;
        protected override void Init()
        {
            GameObject music = new GameObject("MusicSource", typeof(AudioSource));
            music.transform.SetParent(transform);
            MusicSource = music.GetComponent<AudioSource>();

            GameObject sound = new GameObject("SoundSource", typeof(AudioSource));
            sound.transform.SetParent(transform);
            SoundSource = sound.GetComponent<AudioSource>();

            MusicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
            SoundSource.volume = PlayerPrefs.GetFloat("SoundVolume", 1);

            MusicSource.loop = true;
        }
        /// <summary>
        /// 设置音乐音量大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            MusicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
        /// <summary>
        /// 设置音效音量大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundVolume(float volume)
        {
            SoundSource.volume = volume;
            PlayerPrefs.SetFloat("SoundVolume", volume);
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="isCover"></param>
        public void PlaySound(string clipName, bool isCover = false)
        {
            AudioClip clip = ResourceManager.Instance.Load<AudioClip>(clipName);
            PlaySound(clip, isCover);
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="isCover"></param>
        public void PlaySound(AudioClip clip, bool isCover = false)
        {
            if (isCover)
            {
                SoundSource.clip = clip;
                SoundSource.Play();
            }
            else
            {
                SoundSource.PlayOneShot(clip);
            }
        }
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="clipName"></param>
        public void PlayMusic(string clipName)
        {
            AudioClip clip = ResourceManager.Instance.Load<AudioClip>(clipName);
            PlayMusic(clip);
        }
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="clip"></param>
        public void PlayMusic(AudioClip clip)
        {
            MusicSource.clip = clip;
            MusicSource.Play();
        }
    }
}