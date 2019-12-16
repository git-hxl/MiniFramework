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
        public string ResUrl;
        public string AppUrl;
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
                yield return LoadAssetBundle(assetPath);
            }
            else
            {
                assetPath = Application.persistentDataPath + "/" + platform;
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
            yield return httpDownload.Download(ResUrl + "/" + platform + "/config.txt");
            if (httpDownload.isError)
            {
                UpdateFail.Invoke();
                yield break;
            }

            LitJson.JsonData newData = JsonMapper.ToJson(File.ReadAllText(httpDownload.FilePath));
            Version NewVersion = new Version(newData["version"].ToString());

            string configPath = assetPath + "/config.txt";
            if (File.Exists(configPath))
            {
                LitJson.JsonData localData = JsonMapper.ToObject(File.ReadAllText(configPath));
                Version LocalVersion = new Version(localData["version"].ToString());
                Debug.Log("当前版本号：" + LocalVersion + "最新版本号：" + NewVersion);
                if (NewVersion.Major > LocalVersion.Major)
                {
                    Debug.Log("主版本号不一致，请前往平台更新安装包");
                    Application.OpenURL(AppUrl);
                    yield break;
                }
            }
            File.Copy(httpDownload.FilePath, configPath, true);
            yield return DownloadAssetBundle();
        }
        /// <summary>
        /// 下载差异化AssetBundle
        /// </summary>
        /// <returns></returns>
        IEnumerator DownloadAssetBundle()
        {
            httpDownload = new HttpDownload(Application.persistentDataPath + "/Update");
            yield return httpDownload.Download(ResUrl + "/" + platform + "/" + platform);
            if (httpDownload.isError)
            {
                UpdateFail.Invoke();
                yield break;
            }

            string newManifestPath = httpDownload.FilePath;
            string oldManifestPath = assetPath + "/" + platform;
            Dictionary<string, Hash128> newManifest = AssetBundleLoader.LoadABManifest(newManifestPath);
            Dictionary<string, Hash128> oldManifest = AssetBundleLoader.LoadABManifest(oldManifestPath);
            foreach (var item in newManifest)
            {
                if (oldManifest.ContainsKey(item.Key) && oldManifest[item.Key] == item.Value)
                {
                    continue;
                }
                updateFiles.Add(item.Key);
            }
            if (updateFiles.Count > 0)
            {
                httpDownload = new HttpDownload(assetPath);
                foreach (var item in updateFiles)
                {
                    //这里下载差异化AB包
                    string fileUrl = ResUrl + "/" + platform + "/" + item;
                    yield return httpDownload.Download(fileUrl);
                    if (httpDownload.isError)
                    {
                        UpdateFail.Invoke();
                        yield break;
                    }
                }
                updateFiles.Clear();
                File.Copy(newManifestPath, oldManifestPath, true);
            }
            yield return LoadAssetBundle(assetPath);
        }
        /// <summary>
        /// 加载AssetsBundle
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        private IEnumerator LoadAssetBundle(string assetDir)
        {
            Dictionary<string, Hash128> files = AssetBundleLoader.LoadABManifest(assetDir + "/" + platform);
            foreach (var item in files)
            {
                Debug.Log("加载AssetBundle：" + item.Key);
                yield return AssetBundleLoader.LoadAssetBundle(assetDir + "/" + item.Key, (assetBundle) =>
                {
                    ResourceManager.Instance.Bundles.Add(assetBundle);
                });
            }
            UpdateSuccess.Invoke();
        }
    }
}