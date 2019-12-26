using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using LitJson;
namespace MiniFramework
{
    /// <summary>
    /// 检查更新、下载以及加载统一管理组件
    /// </summary>
    public class UpdateComponent : MonoBehaviour
    {
        public bool IsLocal;
        public UnityEvent UpdateSuccess;
        public UnityEvent UpdateFail;
        private string platform;
        private HttpDownload httpDownload;
        private List<string> updateFiles = new List<string>();
        private string assetPath;
        // Use this for initialization
        IEnumerator Start()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    platform = "StandaloneOSX";
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    platform = "StandaloneWindows";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platform = "iOS";
                    break;
                case RuntimePlatform.Android:
                    platform = "Android";
                    break;
            }
            if (IsLocal)
            {
                assetPath = Application.streamingAssetsPath + "/" + platform;
                yield return LoadAssetBundle();
            }
            else
            {
                assetPath = Application.persistentDataPath + "/" + platform;
                //Repair();
                yield return CheckUpdate();
            }
        }
        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckUpdate()
        {
            if(!Directory.Exists(assetPath))
            {
                Directory.CreateDirectory(assetPath);
            }
            httpDownload = new HttpDownload(Application.persistentDataPath + "/Update");
            yield return httpDownload.Download(URLConfig.ResUrl + "/" + platform + "/config.txt");
            if (httpDownload.isError)
            {
                UpdateFail.Invoke();
                yield break;
            }
            string[] downloadConfig = File.ReadAllLines(httpDownload.FilePath);
            Dictionary<string, string> downloadConfigDict = new Dictionary<string, string>();
            foreach (var item in downloadConfig)
            {
                string[] value = item.Split(':');
                downloadConfigDict.Add(value[0], value[1]);
            }

            Version NewVersion = new Version(downloadConfigDict["version"]);
            string configPath = assetPath + "/config.txt";
            string[] localConfig = new string[0];

            if (File.Exists(configPath))
            {
                localConfig = File.ReadAllLines(configPath);

                Version LocalVersion = new Version(localConfig[0].Split(':')[1]);

                Debug.Log("当前版本号：" + LocalVersion + "最新版本号：" + NewVersion);
                if (NewVersion.Major > LocalVersion.Major)
                {
                    Debug.Log("主版本号不一致，请前往平台更新安装包");
                    Application.OpenURL(URLConfig.AppUrl);
                    yield break;
                }
            }
            Dictionary<string, string> localConfigDict = new Dictionary<string, string>();
            foreach (var item in localConfig)
            {
                string[] value = item.Split(':');
                localConfigDict.Add(value[0], value[1]);
            }

            //获取差异化文件
            foreach (var item in downloadConfigDict)
            {
                if(item.Key == "version" || item.Key == platform)
                {
                    continue;
                }
                if(localConfigDict.ContainsKey(item.Key)&&localConfigDict[item.Key] == item.Value)
                {
                    continue;
                }
                updateFiles.Add(item.Key);
            }
            yield return DownloadAssetBundle();
            File.Copy(Application.persistentDataPath + "/Update/config.txt", assetPath + "/config.txt", true);
        }
        /// <summary>
        /// 下载差异化AssetBundle
        /// </summary>
        /// <returns></returns>
        IEnumerator DownloadAssetBundle()
        {
            if (updateFiles.Count > 0)
            {
                httpDownload = new HttpDownload(assetPath);
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
            }
            yield return LoadAssetBundle();
        }
        /// <summary>
        /// 加载AssetsBundle
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        private IEnumerator LoadAssetBundle()
        {
            string[] files = Directory.GetFiles(assetPath);
            foreach (var item in files)
            {
                string extension = Path.GetExtension(item);
                string fileName = Path.GetFileName(item);
                if (extension == "" )
                {
                    Debug.Log("加载AssetBundle：" + fileName);
                    yield return AssetBundleLoader.LoadAssetBundle(assetPath + "/" + fileName, (assetBundle) =>
                    {
                        ResourceManager.Instance.Bundles.Add(assetBundle);
                    });
                }
                if(fileName == "Hotfix.dll")
                {
                    Debug.Log("初始化hotfix");
                    ILRuntimeManager.Instance.dllPath = assetPath + "/" + fileName;
                }
            }
            UpdateSuccess.Invoke();
        }
        /// <summary>
        /// 用于修复客户端
        /// </summary>
        public void Repair()
        {
            if (Directory.Exists(assetPath))
            {
                Directory.Delete(assetPath,true);
            }
        }
    }
}