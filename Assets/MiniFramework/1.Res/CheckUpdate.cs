using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public class CheckUpdate : MonoBehaviour
    {
        //AssetBundle地址
        public string AssetBundleUrl;
        public string curPlatform;
        private string localConfigPath;
        private string internalConfigPath;
        private string localABPath;
        private string internalABPath;
        private string saveManifestPath;
        private List<string> updateFiles = new List<string>();
        private Dictionary<string, string> localInfo;
        private Dictionary<string, string> internalInfo;
        // Use this for initialization
        void Start()
        {
            AssetBundleUrl = "http://localhost:8080";
            saveManifestPath = Application.persistentDataPath + "/Update";
            CheckFiles();
            CheckVersion();
        }

        void CheckFiles()
        {
            localConfigPath = Application.persistentDataPath + "/config.txt";
            internalConfigPath = Application.streamingAssetsPath + "/config.txt";
            if (!FileUtil.IsExitFile(internalConfigPath))
            {
                Debug.LogError("配置文件不存在");
                return;
            }
            if (!FileUtil.IsExitFile(localConfigPath))
            {
                File.Copy(internalConfigPath, localConfigPath);
            }
            localInfo = SerializeUtil.FromJsonFile<Dictionary<string, string>>(localConfigPath);
            internalInfo = SerializeUtil.FromJsonFile<Dictionary<string, string>>(internalConfigPath);
            if (localInfo["platform"] != internalInfo["platform"])
            {
                localInfo["platform"] = internalInfo["platform"];
                SerializeUtil.ToJson(localConfigPath, localInfo);
            }
            curPlatform = localInfo["platform"];
            Debug.Log("平台：" + curPlatform + " 本地版本号：" + localInfo["version"] + " 内部版本号：" + internalInfo["version"]);

            localABPath = Application.persistentDataPath + "/" + curPlatform;
            internalABPath = Application.streamingAssetsPath + "/" + curPlatform;
            if (FileUtil.IsExitDir(internalABPath))
            {
                if (!FileUtil.IsExitDir(localABPath))
                {
                    FileUtil.CreateDir(localABPath);
                    DirectoryInfo internalDirInfo = new DirectoryInfo(internalABPath);
                    FileInfo[] files = internalDirInfo.GetFiles();
                    foreach (var item in files)
                    {
                        Debug.Log("更新文件：" + item.Name);
                        File.Copy(item.FullName, localABPath + "/" + item.Name);
                    }
                }
            }
        }
        void CheckVersion()
        {
            Version curVersion = new Version(localInfo["version"]);
            Version internalVersion = new Version(internalInfo["version"]);
            if (internalVersion > curVersion)
            {
                UpdateLocalFiles();
                localInfo["version"] = internalVersion.ToString();
                SerializeUtil.ToJson(localConfigPath, localInfo);
                curVersion = internalVersion;
            }
            HttpDownload download = new HttpDownload(this, AssetBundleUrl + "/" + curPlatform + "/" + curPlatform, saveManifestPath, () =>
                 {
                     Dictionary<string, Hash128> serverABHash = AssetBundleManager.Instance.LoadABManifest(saveManifestPath + "/" + curPlatform);
                     Dictionary<string, Hash128> localABHash = AssetBundleManager.Instance.LoadABManifest(localABPath + "/" + curPlatform);
                     foreach (var item in serverABHash)
                     {
                         if (!localABHash.ContainsKey(item.Key))
                         {
                             updateFiles.Add(AssetBundleUrl + "/" + curPlatform + "/" + item.Key);
                         }
                         else if (serverABHash[item.Key] != localABHash[item.Key])
                         {
                             updateFiles.Add(AssetBundleUrl + "/" + curPlatform + "/" + item.Key);
                         }
                     }
                     if (updateFiles.Count > 0)
                     {
                         UpdateServerFiles();
                     }
                     else
                     {
                         Debug.Log("无更新列表");
                     }
                 });
        }
        void UpdateLocalFiles()
        {
            if (!FileUtil.IsExitDir(internalABPath))
            {
                return;
            }
            Dictionary<string, Hash128> localABHash = AssetBundleManager.Instance.LoadABManifest(localABPath + "/" + curPlatform);
            Dictionary<string, Hash128> internalABHash = AssetBundleManager.Instance.LoadABManifest(internalABPath + "/" + curPlatform);

            foreach (var item in internalABHash)
            {
                if (!localABHash.ContainsKey(item.Key))
                {
                    updateFiles.Add(item.Key);
                }
                else if (internalABHash[item.Key] != localABHash[item.Key])
                {
                    updateFiles.Add(item.Key);
                }
            }
            foreach (var item in updateFiles)
            {
                Debug.Log("释放本地文件：" + item);
                File.Copy(internalABPath + "/" + item, localABPath + "/" + item, true);
            }
            if (updateFiles.Count > 0)
            {
                File.Copy(internalABPath + "/" + curPlatform, localABPath + "/" + curPlatform, true);
                updateFiles.Clear();
            }
        }
        void UpdateServerFiles()
        {
            HttpDownload downloadAB = new HttpDownload(this, updateFiles, localABPath, () =>
            {
                File.Move(saveManifestPath + "/" + curPlatform, localABPath + "/" + curPlatform);
                Debug.Log("更新网络资源完成");
            });
        }
    }
}