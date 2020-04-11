using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace MiniFramework
{
    public static class FileUtil
    {
        public static IEnumerator ReadStreamingFile(string path, Action<byte[]> callback)
        {
            byte[] result = null;
            if (!path.Contains("://"))
            {
				path = "file://" + path;
            }
			UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
			yield return www.SendWebRequest();
			result = www.downloadHandler.data;
            callback(result);
        }
        public static Dictionary<string,string> ReadConfig(string txt)
        {
            Dictionary<string,string> config = new Dictionary<string, string>();
            string[] lines = txt.Split('\n');
            foreach (var item in lines)
            {
                string[] value = item.Split(':');
                config.Add(value[0],value[1]);
            }
            return config;
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
    }
}