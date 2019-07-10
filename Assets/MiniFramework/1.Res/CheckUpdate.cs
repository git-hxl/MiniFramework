using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public class CheckUpdate : MonoBehaviour
    {
        public string ConfigUrl;
        public string DownloadUrl;
        private string externalDir;
        private string internalDir;
        // Use this for initialization
        void Start()
        {
            externalDir = Application.persistentDataPath + "/AssetBundle";
            internalDir = Application.streamingAssetsPath + "/AssetBundle";
            CheckVersion();
        }

        /// <summary>
        /// 检测版本号
        /// </summary>
        void CheckVersion()
        {
            DirectoryInfo internalDirInfo = new DirectoryInfo(internalDir);
            DirectoryInfo externalDirInfo = new DirectoryInfo(externalDir);
            if (!FileUtil.IsExitDir(externalDir))
            {
                internalDirInfo.MoveTo(externalDir);
                Debug.Log("检测到首次安装,已释放内部资源...");
            }
            else if (FileUtil.IsExitDir(internalDir))
            {
                externalDirInfo.Delete(true);
                internalDirInfo.MoveTo(externalDir);
                Debug.Log("检测到重新安装,已覆盖外部资源...");
            }
            else
            {
                string versionPath = externalDir + "/Config.txt";
                if (!FileUtil.IsExitFile(versionPath))
                {
                    Debug.LogError("未发现版本配置文件!");
                    return;
                }
                // if (FileUtil.IsExitFile(versionPath))
                // {
                //     Dictionary<string, string> internalConfig = SerializeUtil.FromJsonFile<Dictionary<string, string>>(versionPath);
                //     versionPath = externalDir + "/Config.txt";
                //     Dictionary<string, string> externalConfig = SerializeUtil.FromJsonFile<Dictionary<string, string>>(versionPath);
                //     Version internalVersion = new Version(internalConfig["version"]);
                //     Version externalVersion = new Version(externalConfig["version"]);
                //     if (internalVersion > externalVersion)
                //     {
                //         externalDirInfo.Delete(true);
                //         internalDirInfo.MoveTo(externalDir);
                //         Debug.Log("内部版本大于外部版本，已释放内部资源...");
                //     }
                // }
            }
        }

        void CheckFromServer()
        {
            HttpRequest.Get(this, ConfigUrl, (result) =>
            {
                Dictionary<string, string> serverConfig = SerializeUtil.FromJson<Dictionary<string, string>>(result);
                Version serverVersion = new Version(serverConfig["version"]);

                Dictionary<string, string> externalConfig = SerializeUtil.FromJsonFile<Dictionary<string, string>>(externalDir + "/Config.txt");
                Version externalVersion = new Version(externalConfig["version"]);

                if (serverVersion > externalVersion)
                {
                    Debug.Log("检测到新版本");
                }
            });
        }
    }
}