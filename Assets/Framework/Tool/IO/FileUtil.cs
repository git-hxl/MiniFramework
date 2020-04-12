using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework
{
    public static class FileUtil
    {
        public static IEnumerator ReadStreamingFile(string path, Action<DownloadHandler> callback)
        {
            if (!path.Contains("://"))
            {
                path = "file://" + path;
            }
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
            yield return www.SendWebRequest();
            callback(www.downloadHandler);
        }
        public static Dictionary<string, string> TxtToDic(string txt)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txt))
            {
                string[] lines = txt.Split('\n');
                foreach (var item in lines)
                {
                    string[] value = item.Split(':');
                    dict.Add(value[0], value[1]);
                }
            }
            return dict;
        }
        public static Dictionary<string, string> ReadAssetBundleConfig(string path)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                string txt = File.ReadAllText(path);
                dict = TxtToDic(txt);
            }
            return dict;
        }
        public static long GetFileLength(string path)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return 0;
            }
            return file.Length;
        }

        public static Dictionary<string, Hash128> LoadABManifest(AssetBundleManifest manifest)
        {
            Dictionary<string, Hash128> bundlesHash = new Dictionary<string, Hash128>();
            string[] bundlesName = manifest.GetAllAssetBundles();
            for (int i = 0; i < bundlesName.Length; i++)
            {
                Hash128 hash = manifest.GetAssetBundleHash(bundlesName[i]);
                bundlesHash.Add(bundlesName[i], hash);
            }
            return bundlesHash;
        }
    }
}