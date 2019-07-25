using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniFramework
{
    struct Config
    {
        public string version;
        public string platform;
    }
    public class CheckUpdate : MonoBehaviour
    {
        //AssetBundle下载地址
        public string AssetBundleUrl;
        public bool IsTest;

        private HttpDownload httpDownload;
        private string curPlatform;
        private string curVersion;
        private List<string> updateFiles = new List<string>();


        // Use this for initialization
        IEnumerator Start()
        {
            yield return CheckConfig();
            if (IsTest)
            {
                yield return CheckLocalMainfest();
            }
            else
            {
                yield return CheckServerManifest();
            }
            yield return ResManager.Instance.Init(curPlatform);
        }
        IEnumerator CheckConfig()
        {
            string assetsConfigPath = Application.streamingAssetsPath + "/config.txt";
            string dataConfigPath = Application.persistentDataPath + "/config.txt";
            if (!FileUtil.IsExitFile(assetsConfigPath))
            {
                Debug.LogError("配置文件不存在");
                yield break;
            }
            File.Copy(assetsConfigPath, dataConfigPath, true);
            yield return null;
            Config config = SerializeUtil.FromJsonFile<Config>(dataConfigPath);
            curPlatform = config.platform;
            curVersion = config.version;
            Debug.Log("平台:" + curPlatform + " 本地版本号:" + curVersion);
        }
        IEnumerator CheckLocalMainfest()
        {
            Debug.Log("检查更新文件");
            string assetsABPath = Application.streamingAssetsPath + "/" + curPlatform;
            string dataABPath = Application.persistentDataPath + "/" + curPlatform;
            if (!FileUtil.IsExitFile(assetsABPath + "/" + curPlatform))
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
                    Debug.Log("释放本地资源：" + item);
                    File.Copy(assetsABPath + "/" + item, dataABPath + "/" + item, true);
                    yield return null;
                }
                File.Copy(assetsABPath + "/" + curPlatform, dataABPath + "/" + curPlatform, true);
                updateFiles.Clear();
            }
        }
        IEnumerator CheckServerManifest()
        {
            Debug.Log("检查更新文件");
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
                    string url = AssetBundleUrl + "/" + curPlatform + "/" + item;
                    yield return httpDownload.Download(url);
                }
                File.Copy(updateManifestPath, dataManifestPath, true);
                updateFiles.Clear();
            }
        }
    }
}