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
        public string ResUrl;
        public string AppUrl;
        public bool IsLocal;
        public UnityEvent UpdateSuccess;
        public UnityEvent UpdateFail;
        private string platform;
        private HttpDownload httpDownload;
        private List<string> updateFiles = new List<string>();
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
                yield return LoadAssetBundle(Application.streamingAssetsPath + "/" + platform);
            }
            else
            {
                yield return CheckUpdate();
            }
        }
        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckUpdate()
        {
            httpDownload = new HttpDownload(Application.persistentDataPath + "/Update");
            yield return httpDownload.Download(ResUrl + "/" + platform + "/config.txt");
            if (httpDownload.IsError)
            {
                UpdateFail.Invoke();
                yield break;
            }

            LitJson.JsonData newData = SerializeUtil.FromJson(File.ReadAllText(httpDownload.FilePath));
            Version NewVersion = new Version(newData["version"].ToString());

            string configPath = Application.persistentDataPath + "/" + platform + "/config.txt";
            if (File.Exists(configPath))
            {
                LitJson.JsonData localData = SerializeUtil.FromJson(File.ReadAllText(configPath));
                Version LocalVersion = new Version(localData["version"].ToString());
                Debug.Log("当前版本号：" + LocalVersion + "最新版本号：" + NewVersion);
                if (NewVersion != LocalVersion)
                {
                    Debug.Log("版本号不一致，请前往平台更新");
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
            if (httpDownload.IsError)
            {
                UpdateFail.Invoke();
                yield break;
            }

            string newManifestPath = httpDownload.FilePath;
            string oldManifestPath = Application.persistentDataPath + "/" + platform + "/" + platform;
            Dictionary<string, Hash128> newManifest = AssetBundleLoader.LoadABManifest(newManifestPath);
            Dictionary<string, Hash128> oldManifest = AssetBundleLoader.LoadABManifest(oldManifestPath);
            foreach (var item in newManifest)
            {
                if (oldManifest.ContainsKey(item.Key) && oldManifest[item.Key] == oldManifest[item.Key])
                {
                    continue;
                }
                updateFiles.Add(item.Key);
            }
            if (updateFiles.Count > 0)
            {
                httpDownload = new HttpDownload(Application.persistentDataPath + "/" + platform);
                foreach (var item in updateFiles)
                {
                    Debug.Log("下载网络资源：" + item);
                    string fileUrl = ResUrl + "/" + platform + "/" + item;
                    yield return httpDownload.Download(fileUrl);
                    if (httpDownload.IsError)
                    {

                    }
                }
                updateFiles.Clear();
            }
            File.Copy(newManifestPath, oldManifestPath, true);
            yield return LoadAssetBundle(Application.persistentDataPath + "/" + platform);
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