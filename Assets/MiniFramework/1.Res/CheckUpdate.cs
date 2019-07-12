using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public class CheckUpdate : MonoBehaviour
    {
        //版本地址
        public string VersionUrl;
        //AssetBundle目标平台地址
        public string AssetBundleUrl;

        //当前平台
        private string curPlatform;
        //当前版本
        private Version curVersion;
        //内部版本
        private Version internalVersion;
        //服务器版本
        private Version serverVersion;
        private List<string> updateFiles = new List<string>();
        private Dictionary<string, string> localInfo;
        private Dictionary<string, string> internalInfo;
        // Use this for initialization
        void Start()
        {
            InitConfig();
            CheckVersion();
        }
        void InitConfig()
        {

            string localConfigPath = Application.persistentDataPath + "/config.txt";
            string internalConfigPath = Application.streamingAssetsPath + "/config.txt";

            if (!FileUtil.IsExitFile(localConfigPath))
            {
                File.Copy(internalConfigPath, localConfigPath);
            }
            localInfo = SerializeUtil.FromJsonFile<Dictionary<string, string>>(localConfigPath);
            internalInfo = SerializeUtil.FromJsonFile<Dictionary<string, string>>(internalConfigPath);

            if (localInfo["platform"] != internalInfo["platform"])
            {
                Debug.Log("检测到配置文件异常，正重新生成");
                localInfo = internalInfo;
                SerializeUtil.ToJson(localConfigPath, localInfo);
            }
            curPlatform = localInfo["platform"];
            curVersion = new Version(localInfo["version"]);
            internalVersion = new Version(internalInfo["version"]);
            Debug.Log("平台：" + curPlatform + " 本地版本号：" + curVersion + " 内部版本号：" + internalVersion);
        }
        void CheckVersion()
        {
            if (internalVersion >= curVersion)
            {
                UpdateAssetBundle();
            }
            HttpRequest.Get(this, VersionUrl, (res) =>
            {
                serverVersion = new Version(res);
            });

        }
        void UpdateAssetBundle()
        {
            string localPath = Application.persistentDataPath + "/" + curPlatform;
            string internalPath = Application.streamingAssetsPath + "/" + curPlatform;
            if (!FileUtil.IsExitDir(localPath))
            {
                FileUtil.CreateDir(localPath);
                DirectoryInfo internalDirInfo = new DirectoryInfo(internalPath);
                FileInfo[] files = internalDirInfo.GetFiles();
                foreach (var item in files)
                {
                    Debug.Log("更新文件：" + item.Name);
                    File.Copy(item.FullName, localPath + "/" + item.Name);
                }
            }
            else
            {
                Dictionary<string, Hash128> localABHash = AssetBundleManager.Instance.LoadABManifest(localPath + "/" + curPlatform);
                Dictionary<string, Hash128> internalABHash = AssetBundleManager.Instance.LoadABManifest(internalPath + "/" + curPlatform);

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
                    Debug.Log("更新文件：" + item);
                    File.Copy(internalPath + "/" + item, localPath + "/" + item, true);
                }
            }

        }



    }
}