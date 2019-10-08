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
        public static void Open(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                return;
            }
            Process p = new Process();
#if UNITY_EDITOR_WIN
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c start " + path;
#elif UNITY_EDITOR_OSX
            p.StartInfo.FileName = "bash";
            p.StartInfo.Arguments = Application.dataPath + "/MiniFramework/0.Core/Editor/OpenDir.sh" + " " + path;
#endif
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            p.Close();
        }
    }
}