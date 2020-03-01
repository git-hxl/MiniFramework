using System;
using System.Collections;
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

        public static IEnumerator CopyStreamingFile(string sourcePath, string destPath)
        {
			byte[] result = null;
			if (!sourcePath.Contains("://"))
			{
				sourcePath = "file://" + sourcePath;
			}
			UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(sourcePath);
			yield return www.SendWebRequest();
			result = www.downloadHandler.data;
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