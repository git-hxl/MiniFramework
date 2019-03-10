using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class DownloaderManager : MonoSingleton<DownloaderManager>
    {
        public int MaxTogetherNum = 1;
        private string defaultSaveDir;
        private float countTime;
        private int curDownloadNum;
        private string downloadSpeed;
        private long lastTotalSaveLength;
        private Dictionary<string, IDownloader> downloaderHandlers = new Dictionary<string, IDownloader>();
        protected override void OnSingletonInit()
        {
            if (PlatformUtil.IsEditor())
            {
                defaultSaveDir = "D:/Unity3dProjects/MiniFramework/Assets";
            }
            else if (PlatformUtil.IsPC())
            {
                defaultSaveDir = Application.streamingAssetsPath;
            }
            else if (PlatformUtil.IsPhone())
            {
                defaultSaveDir = Application.persistentDataPath;
            }
        }
        /// <summary>
        /// 添加下载任务
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IDownloader AddTask(string url)
        {
            return AddTask(url, defaultSaveDir);
        }
        public IDownloader AddTask(string url, string saveDir)
        {
            IDownloader loader = new Downloader(url, saveDir);
            if (!downloaderHandlers.ContainsKey(loader.GetFileName()))
            {
                loader.DownloadSuccessed += CompleteCallback;
                loader.DownloadFailed += CompleteCallback;
                downloaderHandlers.Add(loader.GetFileName(), loader);
                lastTotalSaveLength += loader.GetCurLength();
            }
            return loader;
        }
        /// <summary>
        /// 获取下载对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDownloader GetTask(string name)
        {
            if (downloaderHandlers.ContainsKey(name))
            {
                return downloaderHandlers[name];
            }
            return null;
        }
        /// <summary>
        /// 获取当前任务数量
        /// </summary>
        /// <returns></returns>
        public int GetTaskCount()
        {
            return downloaderHandlers.Count;
        }
        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="name"></param>
        public void RemoveTask(string name)
        {
            if (downloaderHandlers.ContainsKey(name))
            {
                downloaderHandlers[name].Close();
                downloaderHandlers.Remove(name);
            }
        }
        /// <summary>
        /// 获取下载速度
        /// </summary>
        /// <param name="time">计算间隔时间</param>
        /// <returns></returns>
        public string GetDownloaderSpeed(float time = 0.5f)
        {
            countTime += Time.deltaTime;
            if (countTime >= time)
            {
                long curTotalSaveLength = 0;
                foreach (var item in downloaderHandlers)
                {
                    curTotalSaveLength += item.Value.GetCurLength();
                }
                downloadSpeed = ByteUtil.AutoUnitConversion((curTotalSaveLength - lastTotalSaveLength) / time) + "/S";
                lastTotalSaveLength = curTotalSaveLength;
                countTime = 0;
            }
            return downloadSpeed;
        }
        /// <summary>
        /// 获取下载进度
        /// </summary>
        /// <returns></returns>
        public float GetDownloadProcess()
        {
            float process = 0;
            foreach (var item in downloaderHandlers)
            {
                process += item.Value.GetDownloadProcess();
            }
            return process /= downloaderHandlers.Count;
        }

        public void StartDownload()
        {
            StartCoroutine(downloadIEnumerator());
        }
        IEnumerator downloadIEnumerator()
        {
            foreach (var item in downloaderHandlers)
            {
                if (item.Value.GetDownloadSate() == DownloadState.None || item.Value.GetDownloadSate() == DownloadState.Failed)
                {
                    while (curDownloadNum >= MaxTogetherNum)
                    {
                        yield return null;
                    }
                    curDownloadNum++;
                    item.Value.Start();
                    Debug.Log(item.Key + "：开始下载");
                }
            }
        }
        void CompleteCallback(string name)
        {
            curDownloadNum--;
        }
        public void Close()
        {
            foreach (var item in downloaderHandlers)
            {
                item.Value.Close();
            }
        }
        public void Close(string name)
        {
            if (downloaderHandlers.ContainsKey(name))
            {
                downloaderHandlers[name].Close();
            }
        }
        void OnDestroy()
        {
            Close();
        }
    }
}