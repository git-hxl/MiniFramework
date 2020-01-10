using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
namespace MiniFramework
{
    /// <summary>
    /// 检查更新、下载以及加载统一管理组件
    /// </summary>
    public class UpdateComponent : MonoBehaviour
    {
        public UnityEvent UpdateSuccess;
        public UnityEvent UpdateFail;
        private string platform;//运行平台
        private string resPath;//下载的AB资源保存路径
        private HttpDownload httpDownload;

        private List<string> updateFiles = new List<string>();
        // Use this for initialization

        IEnumerator Start()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    platform = "StandaloneWindows"; break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    platform = "StandaloneOSX"; break;
                case RuntimePlatform.Android:
                    platform = "Android"; break;
                case RuntimePlatform.IPhonePlayer:
                    platform = "iOS"; break;
            }
            resPath = Application.persistentDataPath + "/" + platform;
            if (!Directory.Exists(resPath))
            {
                Directory.CreateDirectory(resPath);
            }
            yield return CheckConfig();
        }
        /// <summary>
        /// 检查配置文件
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckConfig()
        {
            //下载config文件
            httpDownload = new HttpDownload(Application.persistentDataPath);
            string url = URLConfig.ResUrl + "/" + platform + "/config.txt";
            yield return httpDownload.Download(url);
            //下载失败
            if (httpDownload.isError)
            {
                UpdateFail.Invoke();
                yield break;
            }
            //读取最新config
            string[] newConfig = File.ReadAllLines(httpDownload.FilePath);
            Dictionary<string, string> newConfigDict = new Dictionary<string, string>();
            foreach (var item in newConfig)
            {
                string[] value = item.Split(':');
                newConfigDict.Add(value[0], value[1]);
            }
            Debug.Log("最新版本号：" + newConfigDict["version"]);
            Dictionary<string, string> localConfigDict = new Dictionary<string, string>();
            if (File.Exists(resPath + "/config.txt"))
            {
                //读取本地config
                string[] localConfig = File.ReadAllLines(resPath + "/config.txt");
                foreach (var item in localConfig)
                {
                    string[] value = item.Split(':');
                    localConfigDict.Add(value[0], value[1]);
                }
                Debug.Log("本地版本号：" + localConfigDict["version"]);
                Version newVersion = new Version(newConfigDict["version"]);
                Version localVersion = new Version(localConfigDict["version"]);
                //对比版本号
                if (newVersion.Major > localVersion.Major)
                {
                    Debug.LogError("主版本号不一致，请前往平台更新安装包");
                    Application.OpenURL(URLConfig.AppUrl);
                    Application.Quit();
                    yield break;
                }
            }
            //对比Hash值
            foreach (var item in newConfigDict)
            {
                if (item.Key == "version")
                {
                    continue;
                }
                if (localConfigDict.ContainsKey(item.Key) && localConfigDict[item.Key] == item.Value)
                {
                    continue;
                }
                updateFiles.Add(item.Key);
            }
            yield return DownloadAssetBundle();
        }
        /// <summary>
        /// 下载差异化AssetBundle
        /// </summary>
        /// <returns></returns>
        IEnumerator DownloadAssetBundle()
        {
            httpDownload = new HttpDownload(resPath);
            foreach (var item in updateFiles)
            {
                //这里下载差异化AB包
                string fileUrl = URLConfig.ResUrl + "/" + platform + "/" + item;
                yield return httpDownload.Download(fileUrl);
                if (httpDownload.isError)
                {
                    UpdateFail.Invoke();
                    yield break;
                }
            }
            updateFiles.Clear();
            File.Copy(Application.persistentDataPath + "/config.txt", resPath + "/config.txt", true);
            yield return ResourceManager.Instance.LoadAssetBundle(resPath);
            Debug.Log("更新完成");
            UpdateSuccess.Invoke();
        }

        /// <summary>
        /// 用于修复客户端
        /// </summary>
        public void Repair()
        {
            if (Directory.Exists(resPath))
            {
                Directory.Delete(resPath, true);
            }
        }
    }
}