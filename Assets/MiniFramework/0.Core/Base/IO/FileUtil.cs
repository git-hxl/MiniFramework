using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public static class FileUtil
    {
        public static IEnumerator ReadStreamingFile(string path, Action<string> callback)
        {
            string result = "";
            if (path.Contains("://"))
            {
                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
                yield return www.SendWebRequest();
                result = www.downloadHandler.text;
            }
            else
            {
                result = File.ReadAllText(path);
            }
            callback(result);
        }
        public static IEnumerator CopyStreamingFile(string sourcePath, string destPath)
        {
            byte[] result = null;
            if (sourcePath.Contains("://"))
            {
                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(sourcePath);
                yield return www.SendWebRequest();
                result = www.downloadHandler.data;
            }
            else
            {
                result = File.ReadAllBytes(sourcePath);
            }
            File.WriteAllBytes(destPath, result);
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