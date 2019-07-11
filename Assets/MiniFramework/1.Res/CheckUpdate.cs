using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public class CheckUpdate : MonoBehaviour
    {
        //配置信息地址
        public string ConfigUrl;
        //AssetBundle目标平台地址
        public string AssetBundleUrl;
        //当前平台
        private string curPlatform;
        //当前版本
        private Version curVersion;
        // Use this for initialization
        void Start()
        {
            CheckVersion();
        }

        /// <summary>
        /// 检测本地版本号
        /// </summary>
        void CheckVersion()
        {
            DirectoryInfo externalDirInfo = new DirectoryInfo(Application.persistentDataPath);
            if (!FileUtil.IsExitFile(Application.persistentDataPath + "/config.txt"))
            {
                Debug.Log("检测到首次安装");
                if (!FileUtil.IsExitFile(Application.streamingAssetsPath + "/config.txt"))
                {
                    Debug.LogError("未发现版本配置文件!");
                    return;
                }
                File.Copy(Application.streamingAssetsPath + "/config.txt", Application.persistentDataPath + "/config.txt", true);
            }
            Dictionary<string, string> localConfig = SerializeUtil.FromJsonFile<Dictionary<string, string>>(Application.persistentDataPath + "/config.txt");
            curPlatform = localConfig["platform"];
            curVersion = new Version(localConfig["version"]);

            if (!FileUtil.IsExitDir(Application.persistentDataPath + "/" + curPlatform))
            {
                Debug.Log("释放本地资源");
                FileUtil.CreateDir((Application.persistentDataPath + "/" + curPlatform));
                DirectoryInfo internalDirInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + curPlatform);
                FileInfo[] files = internalDirInfo.GetFiles();
                foreach (var item in files)
                {
                    File.Copy(item.FullName, Application.persistentDataPath + "/" + curPlatform + "/" + item.Name, true);
                }
            }
        }
        /// <summary>
        /// 检测服务器版本
        /// </summary>
        void CheckFromServer()
        {

        }
        /// <summary>
        /// 下载平台配置文件对比哈希值
        /// </summary>
        void DownPlaformManifest()
        {

        }
    }
}