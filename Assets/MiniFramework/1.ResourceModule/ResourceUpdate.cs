using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public class ResourceUpdate
    {
        private string resDir;//资源存放目录
        private string configPath;//本地配置文件路径
        private string configUrl;//资源配置信息地址
        private string resUrl;//资源地址
        private string newConfigTxt;
        public float progress;
        public ResourceUpdate()
        {
            resDir = Application.persistentDataPath + "/" + PlatformUtil.GetPlatformName();
            configPath = resDir + "/config.txt";
        }

        /// <summary>
        /// 下载资源配置信息
        /// </summary>
        public IEnumerator DownloadConfig()
        {
            if (!Directory.Exists(resDir))
            {
                Directory.CreateDirectory(resDir);
            }
            Debug.Log("-------------------下载远程配置信息");
            yield return HttpManager.Instance.Get(configUrl, (handler) =>
            {
                newConfigTxt = handler.text;
            });

            if (!string.IsNullOrEmpty(newConfigTxt))
            {
                string localConfig = File.ReadAllText(configPath);
                yield return CheckConfig(newConfigTxt, localConfig);
            }
        }

        /// <summary>
        /// 对比差异化
        /// </summary>
        /// <param name="newConfigTxt"></param>
        /// <param name="localConfigTxt"></param>
        /// <returns></returns>
        private IEnumerator CheckConfig(string newConfigTxt, string localConfigTxt)
        {
            Debug.Log("开始对比差异化文件");

            Dictionary<string, string> newConfig = FileUtil.TxtToDic(newConfigTxt);
            Dictionary<string, string> localConfig = FileUtil.TxtToDic(localConfigTxt);

            List<string> needUpdateFiles = new List<string>();
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
                needUpdateFiles.Add(item.Key);
            }
            yield return UpdateAssets(needUpdateFiles);
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private IEnumerator UpdateAssets(List<string> files)
        {
            Debug.Log("开始更新资源");
            progress = 0f;
            foreach (var item in files)
            {
                string fileUrl = resUrl + "/" + PlatformUtil.GetPlatformName() + "/" + item;
                Downloader downloader = new Downloader(resDir, fileUrl);
                yield return downloader.Download();
                progress += 1f / files.Count;
            }

            File.WriteAllText(configPath, newConfigTxt);
        }
    }

}