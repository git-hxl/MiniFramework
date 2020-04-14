using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MiniFramework.WebRequest;
namespace MiniFramework.Resource
{
    public partial class ResourceManager
    {
        private class ResourceUpdate : IResourceUpdate
        {
            private string platform;//运行平台

            private string assetBundlePath;//下载的AB资源保存路径

            private Dictionary<string, string> localConfig;
            private Dictionary<string, string> newConfig;

            private List<string> updateFiles;

            private ResourceManager resourceManager;

            private IDownload iDownloader;

            public event Action onUpdateCompleted;

            public event Action onUpdateError;
            public ResourceUpdate(ResourceManager resourceManager)
            {
                platform = PlatformUtil.GetPlatformName();
                assetBundlePath = Application.persistentDataPath + "/" + platform;
                if (!Directory.Exists(assetBundlePath))
                {
                    Directory.CreateDirectory(assetBundlePath);
                }
                this.resourceManager = resourceManager;

                localConfig = new Dictionary<string, string>();
                newConfig = new Dictionary<string, string>();
                updateFiles = new List<string>();
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
                string url = Config.Config.Instance.GetConfigUrl.ResUrl + "/" + platform + "/config.txt";
                iDownloader = WebRequestManager.Instance.Downloader(Application.persistentDataPath);
                yield return iDownloader.Get(url);

                //下载失败
                if (iDownloader.isError)
                {
                    onUpdateError?.Invoke();
                    yield break;
                }
                //读取config

                newConfig = FileUtil.ReadAssetBundleConfig(iDownloader.downloadFilePath);

                localConfig = FileUtil.ReadAssetBundleConfig(assetBundlePath + "/config.txt");

                if(newConfig.ContainsKey("version")&&localConfig.ContainsKey("version"))
                {
                    //对比版本号
                    if (newConfig["version"] != localConfig["version"])
                    {
                        Debug.LogError("版本号不一致，请前往平台更新安装包");
                        Application.OpenURL(Config.Config.Instance.GetConfigUrl.AppUrl);
                        Application.Quit();
                        yield break;
                    }
                }
                //对比Hash值
                foreach (var item in newConfig)
                {
                    if (item.Key == "version")
                    {
                        continue;
                    }
                    if (localConfig.ContainsKey(item.Key) && localConfig[item.Key] == item.Value)
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
                iDownloader = WebRequestManager.Instance.Downloader(assetBundlePath);
                foreach (var item in updateFiles)
                {
                    //这里下载差异化AB包
                    string fileUrl = Config.Config.Instance.GetConfigUrl.ResUrl + "/" + platform + "/" + item;
                    yield return iDownloader.Get(fileUrl);
                    if (iDownloader.isError)
                    {
                        onUpdateError?.Invoke();
                        yield break;
                    }
                }
                updateFiles.Clear();
                File.Copy(Application.persistentDataPath + "/config.txt", assetBundlePath + "/config.txt", true);
                onUpdateCompleted?.Invoke();
            }
            /// <summary>
            /// 修复文件错误
            /// </summary>
            public void RepairErro()
            {
                if (Directory.Exists(assetBundlePath))
                {
                    Directory.Delete(assetBundlePath, true);
                }
            }
        }
    }
}
