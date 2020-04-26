using UnityEngine;
namespace MiniFramework.Audio
{
    public interface IAudioManager
    {
        /// <summary>
        /// 设置总音量大小
        /// </summary>
        /// <param name="volume"></param>
        void SetTotalVolume(float volume);
        /// <summary>
        /// 设置音乐音量大小
        /// </summary>
        /// <param name="volume"></param>
        void SetMusicVolume(float volume);
        /// <summary>
        /// 设置音效音量大小
        /// </summary>
        /// <param name="volume"></param>
        void SetSoundVolume(float volume);
        /// <summary>
        /// 静音
        /// </summary>
        void DisableVolume();
        /// <summary>
        /// 恢复静音
        /// </summary>
        void EnableVolume();
        /// <summary>
        /// 加载音效并播放
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        AudioSource PlaySound(string clipName, bool isOnShot = false);
        /// <summary>
        /// 加载音效并播放
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        void PlaySoundAtPoint(string clipName, Vector3 pos);
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip"></param>
        /// <returns></returns>
        AudioSource PlaySound(AudioClip audioClip, bool isOnShot = false);
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip"></param>
        /// <returns></returns>
        void PlaySoundAtPoint(AudioClip audioClip, Vector3 pos);
        /// <summary>
        /// 加载音乐并播放
        /// </summary>
        /// <param name="clipName"></param>
        /// <returns></returns>
        AudioSource PlayMusic(string clipName);
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="audioClip"></param>
        /// <returns></returns>
        AudioSource PlayMusic(AudioClip audioClip);
        /// <summary>
        /// 暂停当前音乐
        /// </summary>
        void StopCurMusic();
        /// <summary>
        /// 播放当前音乐
        /// </summary>
        void PlayCurMusic();
    }
}
