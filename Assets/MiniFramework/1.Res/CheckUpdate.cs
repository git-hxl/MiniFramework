using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniFramework
{
    class Config
    {
        public string version;
        public string platform;
    }
    [Serializable]
    public class CheckUpdate
    {
        //AssetBundle下载地址
        public string AssetBundleUrl;

        private HttpDownload httpDownload;
        private string curPlatform;
        private string curVersion;
        private List<string> updateFiles = new List<string>();

        public string CurPlatform { get { return curPlatform; } }
        public float CurProgress { get { return httpDownload.Progress; } }

        public IEnumerator Check()
        {
            yield return CheckConfig();

            yield return CheckLocalMainfest();

            yield return CheckServerManifest();
        }
        IEnumerator CheckConfig()
        {
            string assetsConfigPath = Application.streamingAssetsPath + "/config.txt";
            string dataConfigPath = Application.persistentDataPath + "/config.txt";
            if (!File.Exists(assetsConfigPath))
            {
                Debug.LogError("未生成本地配置文件！");
                yield break;
            }
            Config assetsConfig = SerializeUtil.FromJsonFile<Config>(assetsConfigPath);
            curPlatform = assetsConfig.platform;
            curVersion = assetsConfig.version;
            Debug.Log("平台:" + curPlatform + " 本地版本号:" + curVersion);
            if (!File.Exists(dataConfigPath))
            {
                yield return DisposeLocalRes();
            }
            else
            {
                Config dataConfig = SerializeUtil.FromJsonFile<Config>(dataConfigPath);
                if (dataConfig.version != assetsConfig.version)
                {
                    yield return DisposeLocalRes();
                }
            }
            File.Copy(assetsConfigPath, dataConfigPath, true);
        }

        IEnumerator DisposeLocalRes()
        {
            string assetsABPath = Application.streamingAssetsPath + "/" + curPlatform;
            string dataABPath = Application.persistentDataPath + "/" + curPlatform;
            if (!File.Exists(assetsABPath + "/" + curPlatform))
            {
                yield break;
            }
            if (Directory.Exists(dataABPath))
            {
                Directory.Delete(dataABPath, true);
                yield return null;
            }
            FileUtil.CreateDir(dataABPath);
            Dictionary<string, Hash128> assetsManifest = AssetBundleLoader.LoadABManifest(assetsABPath + "/" + curPlatform);
            foreach (var item in assetsManifest)
            {
                Debug.Log("释放本地资源：" + item.Key);
                File.Copy(assetsABPath + "/" + item.Key, dataABPath + "/" + item.Key, true);
                yield return null;
            }
            File.Copy(assetsABPath + "/" + curPlatform, dataABPath + "/" + curPlatform, true);
        }


        IEnumerator CheckLocalMainfest()
        {
            Debug.Log("检查更新本地文件");
            string assetsABPath = Application.streamingAssetsPath + "/" + curPlatform;
            string dataABPath = Application.persistentDataPath + "/" + curPlatform;
            if (!File.Exists(assetsABPath + "/" + curPlatform))
            {
                yield break;
            }
            FileUtil.CreateDir(dataABPath);
            Dictionary<string, Hash128> assetsManifest = AssetBundleLoader.LoadABManifest(assetsABPath + "/" + curPlatform);
            Dictionary<string, Hash128> dataManifest = AssetBundleLoader.LoadABManifest(dataABPath + "/" + curPlatform);
            foreach (var item in assetsManifest)
            {
                if (dataManifest.ContainsKey(item.Key) && dataManifest[item.Key] == assetsManifest[item.Key])
                {
                    continue;
                }
                updateFiles.Add(item.Key);
            }
            if (updateFiles.Count > 0)
            {
                foreach (var item in updateFiles)
                {
                    Debug.Log("更新本地资源：" + item);
                    File.Copy(assetsABPath + "/" + item, dataABPath + "/" + item, true);
                    yield return null;
                }
                File.Copy(assetsABPath + "/" + curPlatform, dataABPath + "/" + curPlatform, true);
                updateFiles.Clear();
            }
        }

        IEnumerator CheckServerManifest()
        {
            Debug.Log("检查更新服务器文件");
            string downloadPath = AssetBundleUrl + "/" + curPlatform + "/" + curPlatform;
            string savePath = Application.persistentDataPath + "/Update";
            httpDownload = new HttpDownload(savePath, null);
            yield return httpDownload.Download(downloadPath);
            string updateManifestPath = httpDownload.FilePath;
            string dataManifestPath = Application.persistentDataPath + "/" + curPlatform + "/" + curPlatform;
            Dictionary<string, Hash128> downloadManifes = AssetBundleLoader.LoadABManifest(updateManifestPath);
            Dictionary<string, Hash128> dataManifest = AssetBundleLoader.LoadABManifest(dataManifestPath);
            foreach (var item in downloadManifes)
            {
                if (dataManifest.ContainsKey(item.Key) && dataManifest[item.Key] == downloadManifes[item.Key])
                {
                    continue;
                }
                updateFiles.Add(item.Key);
            }
            if (updateFiles.Count > 0)
            {
                savePath = Application.persistentDataPath + "/" + curPlatform;
                httpDownload = new HttpDownload(savePath, null);
                foreach (var item in updateFiles)
                {
                    Debug.Log("更新网络资源：" + item);
                    string url = AssetBundleUrl + "/" + curPlatform + "/" + item;
                    yield return httpDownload.Download(url);
                }
                File.Copy(updateManifestPath, dataManifestPath, true);
                updateFiles.Clear();
            }
        }
    }
}