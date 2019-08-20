using UnityEngine;
namespace MiniFramework
{
    public class AudioManager : MonoSingleton<AudioManager>
    {

        private AudioSource MusicSource;
        private AudioSource SoundSource;
        private float musicVolume;
        private float soundVolume;
        protected override void Awake()
        {
            base.Awake();

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
        public void PlaySound(string clipName, bool isCover = false)
        {
            AudioClip clip = ResManager.Instance.Load(clipName) as AudioClip;
            PlaySound(clip, isCover);
        }
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
        public void PlayMusic(string clipName)
        {
            AudioClip clip = ResManager.Instance.Load(clipName) as AudioClip;
            PlayMusic(clip);
        }
        public void PlayMusic(AudioClip clip)
        {
            MusicSource.clip = clip;
            MusicSource.Play();
        }
    }
}