using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework.Resource
{
    public partial class ResourceManager
    {
        private class ResourceUpdate : IResourceUpdate
        {
            private string platform;//运行平台

            private string resPath;//下载的AB资源保存路径

            private HttpDownload httpDownload;

            private List<string> updateFiles = new List<string>();

            private ResourceManager resourceManager;
            public ResourceUpdate(ResourceManager resourceManager)
            {
                platform = PlatformUtil.GetPlatformName();
                resPath = Application.persistentDataPath + "/" + platform;
                if (!Directory.Exists(resPath))
                {
                    Directory.CreateDirectory(resPath);
                }
                this.resourceManager = resourceManager;
            }
            /// <summary>
            /// 检查配置文件
            /// </summary>
            /// <returns></returns>
            public IEnumerator CheckConfig()
            {
                if (resourceManager.readType != ReadType.FromPersistentPath)
                {
                    yield break;
                }
                //下载config文件
                httpDownload = new HttpDownload(Application.persistentDataPath);
                string url = Config.Config.URL.ResUrl + "/" + platform + "/config.txt";
                yield return httpDownload.Download(url);
                //下载失败
                if (httpDownload.isError)
                {
                    resourceManager.updateFailCallback?.Invoke();
                    yield break;
                }
                //读取最新config
                Dictionary<string, string> newConfigDict = FileUtil.ReadConfig(File.ReadAllText(httpDownload.FilePath));
                Debug.Log("最新版本号：" + newConfigDict["version"]);
                Dictionary<string, string> localConfigDict = new Dictionary<string, string>();
                if (File.Exists(resPath + "/config.txt"))
                {
                    //读取本地config
                    localConfigDict = FileUtil.ReadConfig(File.ReadAllText(resPath + "/config.txt"));
                    Debug.Log("本地版本号：" + localConfigDict["version"]);
                    Version newVersion = new Version(newConfigDict["version"]);
                    Version localVersion = new Version(localConfigDict["version"]);
                    //对比版本号
                    if (newVersion.Major > localVersion.Major)
                    {
                        Debug.LogError("主版本号不一致，请前往平台更新安装包");
                        Application.OpenURL(Config.Config.URL.AppUrl);
                        Application.Quit();
                        yield break;
                    }
                }
                //对比Hash值
                foreach (var item in newConfigDict)
                {
                    if (item.Key == "version")
                    {
                        continue;
                    }
                    if (localConfigDict.ContainsKey(item.Key) && localConfigDict[item.Key] == item.Value)
                    {
                        continue;
                    }
                    updateFiles.Add(item.Key);
                }
                yield return DownloadNewAssetBundle();
            }
            /// <summary>
            /// 下载差异化资源
            /// </summary>
            /// <returns></returns>
            public IEnumerator DownloadNewAssetBundle()
            {
                httpDownload = new HttpDownload(resPath);
                foreach (var item in updateFiles)
                {
                    //这里下载差异化AB包
                    string fileUrl = Config.Config.URL.ResUrl + "/" + platform + "/" + item;
                    yield return httpDownload.Download(fileUrl);
                    if (httpDownload.isError)
                    {
                        resourceManager.updateFailCallback?.Invoke();
                        yield break;
                    }
                }
                updateFiles.Clear();
                File.Copy(Application.persistentDataPath + "/config.txt", resPath + "/config.txt", true);
                Debug.Log("更新完成");
                resourceManager.updateSuccessCallback?.Invoke();
            }
            /// <summary>
            /// 修复文件错误
            /// </summary>
            public void RepairErro()
            {
                if (Directory.Exists(resPath))
                {
                    Directory.Delete(resPath, true);
                }
            }
        }
    }
}
