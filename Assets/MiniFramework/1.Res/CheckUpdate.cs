using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniFramework
{
    [Serializable]
    public class CheckUpdate
    {
        //AssetBundle下载地址
        public string AssetBundleUrl;
        public string Platform;
        public string Version;
        private HttpDownload httpDownload;
        private List<string> updateFiles = new List<string>();
        public IEnumerator Check()
        {
            yield return CheckConfig();

            yield return CheckLocalMainfest();

            yield return CheckServerManifest();
        }
        IEnumerator CheckConfig()
        {
            string assetsConfigPath = "file://" + Application.streamingAssetsPath + "/config.txt";
            yield return FileUtil.ReadStreamingFile(assetsConfigPath, (result) =>
             {
                 LitJson.JsonData jsonData = SerializeUtil.FromJson(result);
                 Platform = jsonData["platform"].ToString();
                 Version = jsonData["version"].ToString();
                 Debug.Log("当前版本号:" + Version + " 平台:" + Platform);
             });
        }
        IEnumerator CheckLocalMainfest()
        {
            Debug.Log("检查更新本地文件");
            string assetsABPath = Application.streamingAssetsPath + "/" + Platform;
            string dataABPath = Application.persistentDataPath + "/" + Platform;
            Directory.CreateDirectory(dataABPath);
            Dictionary<string, Hash128> assetsManifest = AssetBundleLoader.LoadABManifest(assetsABPath + "/" + Platform);
            Dictionary<string, Hash128> dataManifest = AssetBundleLoader.LoadABManifest(dataABPath + "/" + Platform);
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
                    yield return FileUtil.CopyStreamingFile(assetsABPath + "/" + item, dataABPath + "/" + item);
                }
                yield return FileUtil.CopyStreamingFile(assetsABPath + "/" + Platform, dataABPath + "/" + Platform);
                updateFiles.Clear();
            }
        }

        IEnumerator CheckServerManifest()
        {
            Debug.Log("检查更新服务器文件");
            string downloadPath = AssetBundleUrl + "/" + Platform + "/" + Platform;
            string savePath = Application.persistentDataPath + "/Update";
            httpDownload = new HttpDownload(savePath);
            yield return httpDownload.Download(downloadPath);
            string updateManifestPath = httpDownload.FilePath;
            string dataManifestPath = Application.persistentDataPath + "/" + Platform + "/" + Platform;
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
                savePath = Application.persistentDataPath + "/" + Platform;
                httpDownload = new HttpDownload(savePath);
                foreach (var item in updateFiles)
                {
                    Debug.Log("更新网络资源：" + item);
                    string url = AssetBundleUrl + "/" + Platform + "/" + item;
                    yield return httpDownload.Download(url);
                }
                File.Copy(updateManifestPath, dataManifestPath, true);
                updateFiles.Clear();
            }
        }
    }
}