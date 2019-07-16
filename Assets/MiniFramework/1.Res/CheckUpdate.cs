using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public class CheckUpdate : MonoBehaviour
    {
        //AssetBundle下载地址
        public string AssetBundleUrl;
        public bool IsTest;
        public HttpDownload HttpDownload;

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

            ResManager.Instance.Init(curPlatform);
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
            Dictionary<string, string> config = SerializeUtil.FromJsonFile<Dictionary<string, string>>(dataConfigPath);
            curPlatform = config["platform"];
            curVersion = config["version"];
            Debug.Log("平台:" + curPlatform + " 本地版本号:" + curVersion);
        }
        IEnumerator CheckLocalMainfest()
        {
            string assetsABPath = Application.streamingAssetsPath + "/" + curPlatform;
            string dataABPath = Application.persistentDataPath + "/" + curPlatform;
            if (!FileUtil.IsExitFile(assetsABPath + "/" + curPlatform))
            {
                yield break;
            }
            FileUtil.CreateDir(dataABPath);
            Dictionary<string, Hash128> assetsManifest =AssetBundleLoader.LoadABManifest(assetsABPath + "/" + curPlatform);
            Dictionary<string, Hash128> dataManifest = AssetBundleLoader.LoadABManifest(dataABPath + "/" + curPlatform);
            foreach (var item in assetsManifest)
            {
                if (dataManifest.ContainsKey(item.Key) && dataManifest[item.Key] == assetsManifest[item.Key])
                {
                    continue;
                }
                updateFiles.Add(item.Key);
            }
            foreach (var item in updateFiles)
            {
                Debug.Log("释放本地资源：" + item);
                File.Copy(assetsABPath + "/" + item, dataABPath + "/" + item, true);
                yield return null;
            }
            File.Copy(assetsABPath + "/" + curPlatform, dataABPath + "/" + curPlatform, true);
            updateFiles.Clear();
        }
        IEnumerator CheckServerManifest()
        {
            string downloadPath = AssetBundleUrl + "/" + curPlatform + "/" + curPlatform;
            string savePath = Application.persistentDataPath + "/Update";
            HttpDownload = new HttpDownload(savePath, null);
            yield return HttpDownload.Download(downloadPath);
            string updateManifestPath = HttpDownload.FilePath;
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
            savePath = Application.persistentDataPath + "/" + curPlatform;
            HttpDownload = new HttpDownload(savePath, null);
            foreach (var item in updateFiles)
            {
                string url = AssetBundleUrl + "/" + curPlatform + "/" + item;
                yield return HttpDownload.Download(url);
            }
            File.Copy(updateManifestPath, dataManifestPath, true);
            updateFiles.Clear();
            Debug.Log("更新完成");
        }
    }
}